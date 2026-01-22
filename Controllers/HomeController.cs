using ElektronikSatis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElektronikSatis.Controllers
{
    public class HomeController : Controller
    {
        #region view
        // GET: Home
        public ActionResult Index()
        {
            // 1. Depodan tüm ürünleri çek
            var tumUrunler = UrunDeposu.UrunleriGetir();

            // 2. Fiyata göre sırala (En ucuzdan pahalıya)
            // Anasayfada karışık veya ucuz ürünleri göstermek iyidir
            var vitrinUrunleri = tumUrunler.OrderBy(x => x.Fiyat).ToList();

            // 3. Listeyi View'a (Sayfaya) gönder
            // (İşte bu kısmı yapmadığımız için hata alıyordun)
            return View(vitrinUrunleri);
        }
        //-------------Profil resmi ayarları--------------------



        public ActionResult Profil()
        {
            string girisYapanIsim = User.Identity.Name;
            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == girisYapanIsim);

            if (aktifKullanici == null) return RedirectToAction("Logout", "Login");

            // Sipariş Sayısı
            int siparisSayisi = aktifKullanici.Siparisler != null ? aktifKullanici.Siparisler.Count : 0;
            ViewBag.SiparisAdedi = siparisSayisi;

            // --- YENİ KISIM: FAVORİLERİ HAZIRLA ---
            var favoriUrunler = new List<Models.Urun>();
            if (aktifKullanici.FavoriUrunIdleri != null && aktifKullanici.FavoriUrunIdleri.Count > 0)
            {
                // Tüm ürün kataloğunu çek
                var tumUrunler = Models.UrunDeposu.UrunleriGetir();

                // Favori ID'leri ile eşleşen ürünleri bul
                favoriUrunler = tumUrunler.Where(x => aktifKullanici.FavoriUrunIdleri.Contains(x.Id)).ToList();
            }
            ViewBag.Favoriler = favoriUrunler;
            // --------------------------------------

            return View(aktifKullanici.ProfilBilgisi);
        }

        [HttpPost]
        public ActionResult AdresEkle(Models.Adres yeniAdres)
        {
            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici != null)
            {
                // Listeyi başlat (Eğer null ise)
                if (aktifKullanici.ProfilBilgisi.Adresler == null)
                    aktifKullanici.ProfilBilgisi.Adresler = new List<Models.Adres>();

                // Yeni adresi ekle
                aktifKullanici.ProfilBilgisi.Adresler.Add(yeniAdres);

                // Kaydet
                Models.VeriYoneticisi.VerileriKaydet(liste);

                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Oturum hatası." });
        }

        // Adres Silme (Bonus)
        [HttpPost]
        public ActionResult AdresSil(string baslik)
        {
            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici != null && aktifKullanici.ProfilBilgisi.Adresler != null)
            {
                var silinecek = aktifKullanici.ProfilBilgisi.Adresler.FirstOrDefault(x => x.Baslik == baslik);
                if (silinecek != null)
                {
                    aktifKullanici.ProfilBilgisi.Adresler.Remove(silinecek);
                    Models.VeriYoneticisi.VerileriKaydet(liste);
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult UploadProfileImage()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    // Dosya işlemleri aynı
                    var fileName = "profile_" + User.Identity.Name + ".jpg"; // Herkesin resmi kendi adıyla olsun
                    var path = System.IO.Path.Combine(Server.MapPath("~/Content/image"), fileName);
                    file.SaveAs(path);

                    // --- VERİ GÜNCELLEME KISMI (ÇOKLU KULLANICI UYUMLU) ---

                    // A) Listeyi çek
                    var liste = Models.VeriYoneticisi.VerileriGetir();

                    // B) Giriş yapan kullanıcıyı bul
                    var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

                    if (aktifKullanici != null)
                    {
                        // C) Sadece o kullanıcının resim yolunu güncelle
                        string yeniYol = "/Content/image/" + fileName + "?v=" + DateTime.Now.Ticks;
                        aktifKullanici.ProfilBilgisi.ResimYolu = yeniYol;

                        // D) Tüm listeyi tekrar kaydet
                        Models.VeriYoneticisi.VerileriKaydet(liste);

                        return Json(new { success = true, newUrl = yeniYol });
                    }
                }
            }
            return Json(new { success = false, message = "Kullanıcı bulunamadı veya dosya hatası." });
        }

        // Profil bölümündeki kullanıcı verilerinin kaydı için
        [HttpPost]
        public ActionResult ProfilGuncelle(Models.Profil gelenVeri)
        {
            // 1. Tüm listeyi çek
            var liste = Models.VeriYoneticisi.VerileriGetir();

            // 2. Giriş yapan kullanıcıyı bul
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici == null)
            {
                return Json(new { success = false, message = "Oturum hatası. Lütfen tekrar giriş yapın." });
            }

            // 3. Bilgileri Güncelle (Email'i değiştirmiyoruz, o kimliktir)
            aktifKullanici.ProfilBilgisi.Ad = gelenVeri.Ad;
            aktifKullanici.ProfilBilgisi.Soyad = gelenVeri.Soyad;
            aktifKullanici.ProfilBilgisi.Telefon = gelenVeri.Telefon;
            aktifKullanici.ProfilBilgisi.Sehir = gelenVeri.Sehir;
            aktifKullanici.ProfilBilgisi.DogumTarihi = gelenVeri.DogumTarihi;

            // 4. Veritabanına (JSON) Kaydet
            Models.VeriYoneticisi.VerileriKaydet(liste);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ChangePassword(string currentPass, string newPass)
        {
            // A) Listeyi çek
            var liste = Models.VeriYoneticisi.VerileriGetir();

            // B) Giriş yapan kullanıcıyı bul
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici == null) return Json(new { success = false, message = "Oturum hatası." });

            // Eski şifre doğru mu?
            if (aktifKullanici.Sifre != currentPass)
            {
                return Json(new { success = false, message = "Mevcut şifreniz hatalı!" });
            }

            // C) Şifreyi güncelle
            aktifKullanici.Sifre = newPass;

            // D) Listeyi kaydet
            Models.VeriYoneticisi.VerileriKaydet(liste);

            return Json(new { success = true });
        }

        //-------------Profil resmi ayarları--------------------
        public ActionResult Arama(string q)
        {
            // 1. Tüm ürünleri getir
            var tumUrunler = UrunDeposu.UrunleriGetir();

            // 2. Eğer arama kutusu boşsa veya null ise boş liste döndür
            if (string.IsNullOrEmpty(q))
            {
                return View(new List<ElektronikSatis.Models.Urun>());
            }

            // 3. Filtreleme Yap (Büyük/Küçük harf duyarlılığını kaldırmak için ToLower kullanıyoruz)
            // Hem ürün ADINDA hem de AÇIKLAMASINDA arar.
            var sonuc = tumUrunler.Where(x =>
                x.Ad.ToLower().Contains(q.ToLower()) ||
                (x.Aciklama != null && x.Aciklama.ToLower().Contains(q.ToLower()))
            ).ToList();

            // 4. Aranan kelimeyi sayfaya taşı (Başlıkta göstermek için)
            ViewBag.ArananKelime = q;

            return View(sonuc);
        }

        public ActionResult ViewPersonels()
        {
            return View();
        }

        public ActionResult UrunDetay(int id)
        {
            // 1. Tüm ürünleri çek
            var tumUrunler = ElektronikSatis.Models.UrunDeposu.UrunleriGetir();

            // 2. Seçilen ürünü bul
            var secilenUrun = tumUrunler.FirstOrDefault(x => x.Id == id);

            if (secilenUrun == null) return RedirectToAction("Index");

            // 3. BENZER ÜRÜNLER ALGORİTMASI (Basit Versiyon)
            // Mantık: Şu anki ürün HARİÇ, rastgele 4 tane ürün seç.
            // (Guid.NewGuid() metodu listeyi rastgele karıştırmak için harika bir hiledir)
            var benzerUrunler = tumUrunler
                                .Where(x => x.Id != id) // Kendisini hariç tut
                                .OrderBy(x => Guid.NewGuid()) // Karıştır
                                .Take(4) // 4 tane al
                                .ToList();

            // Bu listeyi sayfaya (ViewBag ile) taşıyoruz
            ViewBag.BenzerUrunler = benzerUrunler;

            return View(secilenUrun);
        }

        // TÜM ÜRÜNLER SAYFASI
        public ActionResult TumUrunler()
        {
            // Depodaki her şeyi getir
            var hepsi = UrunDeposu.UrunleriGetir();

            // İstersen burada fiyata göre sıralama da yapabilirsin:
            // var hepsi = UrunDeposu.UrunleriGetir().OrderBy(x => x.Fiyat).ToList();

            return View(hepsi);
        }

        // MESAJ KUTUSU (Inbox) - ARTIK KİŞİYE ÖZEL JSON'DAN OKUYOR
        public ActionResult Mesajlarim()
        {
            // 1. Tüm listeyi çek
            var liste = Models.VeriYoneticisi.VerileriGetir();

            // 2. Giriş yapan kullanıcıyı bul
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            // 3. Kullanıcının mesajlarını al (Yoksa boş liste gönder)
            var mesajlar = aktifKullanici != null ? aktifKullanici.Mesajlar : new List<Models.Mesaj>();

            return View(mesajlar);
        }

        // MESAJ OKUMA (Detail) - ARTIK KİŞİYE ÖZEL JSON'DAN BULUYOR
        public ActionResult MesajOku(int id)
        {
            // 1. Tüm listeyi çek
            var liste = Models.VeriYoneticisi.VerileriGetir();

            // 2. Giriş yapan kullanıcıyı bul
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici == null || aktifKullanici.Mesajlar == null)
            {
                return RedirectToAction("Mesajlarim");
            }

            // 3. Kullanıcının mesajları içinden ID'si tutan mesajı bul
            var secilenMesaj = aktifKullanici.Mesajlar.FirstOrDefault(x => x.Id == id);

            if (secilenMesaj == null) return RedirectToAction("Mesajlarim");

            return View(secilenMesaj);
        }

        // 1. BİLGİSAYARLAR
        public ActionResult Bilgisayarlar()
        {
            var tumUrunler = UrunDeposu.UrunleriGetir();

            // Formda "Bilgisayar" kategorisi olarak işaretlenen ürünleri Bilgisayarlar kategorisinde sergile
            var bilgisayarlar = tumUrunler.Where(x => x.Kategori == "Bilgisayar").ToList();

            return View(bilgisayarlar);
        }

        // 2. CEP TELEFONLARI
        public ActionResult Cep_Telefonları()
        {
            var tumUrunler = UrunDeposu.UrunleriGetir();

            // Formda "Telefon" kategorisi olarak işaretlenen ürünleri Telefonlar kategorisinde sergile
            var telefonlar = tumUrunler.Where(x => x.Kategori == "Telefon").ToList();

            return View(telefonlar);
        }

        // Formda "Switch" kategorisi olarak işaretlenen ürünleri Switchler kategorisinde sergile
        public ActionResult Switchler()
        {
            var tumUrunler = UrunDeposu.UrunleriGetir();

            // İsminde Switch, Catalyst veya CloudEngine geçenleri getir
            var switchler = tumUrunler.Where(x => x.Kategori == "Switch").ToList();

            return View(switchler);
        }

        // Formda "Router" kategorisi olarak işaretlenen ürünleri Routerlar kategorisinde sergile
        public ActionResult Routerlar()
        {
            var tumUrunler = UrunDeposu.UrunleriGetir();

            // İsminde Router, NetEngine veya ISR geçenleri getir
            var routerlar = tumUrunler.Where(x =>
                x.Ad.Contains("Router") ||
                x.Ad.Contains("NetEngine") ||
                x.Ad.Contains("ISR")
            ).ToList();

            return View(routerlar);
        }

        // CANLI ARAMA İÇİN JSON DÖNEN METOT
        public JsonResult CanliArama(string term)
        {
            // 1. Tüm ürünleri depodan çek
            var tumUrunler = ElektronikSatis.Models.UrunDeposu.UrunleriGetir();

            // 2. Arama terimine göre filtrele (Büyük/Küçük harf duyarsız)
            var sonuclar = tumUrunler.Where(x => x.Ad.ToLower().Contains(term.ToLower())).ToList();

            // 3. Sadece gerekli verileri (Ad, Resim, Fiyat, ID) seçip gönder (Tüm veriyi gönderme, hafif olsun)
            var sonucListesi = sonuclar.Select(x => new
            {
                x.Id,
                x.Ad,
                x.ResimYolu,
                Fiyat = x.Fiyat.ToString("N0") + " TL"
            });

            return Json(sonucListesi, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Sepet()
        {
            return View();
        }

        public ActionResult Blank()
        {
            return View();
        }

        public ActionResult İletisim()
        {
            return View();
        }

        public ActionResult Gecmis_Siparislerim()
        {
            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            // Kullanıcının siparişlerini View'a gönder
            var siparisler = aktifKullanici != null ? aktifKullanici.Siparisler : new List<Models.Siparis>();

            return View(siparisler);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }

        // ÖDEME SAYFASI
        public ActionResult SiparisiTamamla()
        {
            // Kullanıcının adreslerini sayfaya taşıyoruz
            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici != null)
            {
                // Profil bilgisini (içinde adresler var) sayfaya gönder
                return View(aktifKullanici.ProfilBilgisi);
            }

            return RedirectToAction("Index");
        }


        // 2. Kayıt formundaki "Hesap Oluştur" butonuna basıldığında bu metot çalışır ve verileri İŞLER.
        [HttpPost]
        public ActionResult Register(string FullName, string Email, string Password, string RepeatPassword)
        {
            // Şifrelerin eşleşip eşleşmediğini kontrol et
            if (Password != RepeatPassword)
            {
                TempData["ErrorMessage"] = "Girdiğiniz şifreler eşleşmiyor.";
                return View(); // Hata varsa, aynı sayfada kal ve mesajı göster
            }

            // --- VERİTABANINA KAYIT SİMÜLASYONU ---
            // Gerçek bir projede bu kısımda gelen bilgilerle veritabanına yeni bir kullanıcı eklenir.

            // Her şey yolundaysa, kullanıcıyı başarılı bir mesajla giriş sayfasına yönlendir.
            TempData["SuccessMessage"] = "Hesabınız başarıyla oluşturuldu! Lütfen giriş yapın.";
            return RedirectToAction("Index", "Login");
        }

        // ForgotPassword metodu artık sınıfın İÇİNDE ve doğru yerde.

        #endregion

        [HttpPost]
        public ActionResult SiparisKaydet(Models.Siparis yeniSiparis)
        {
            // 1. Listeyi Çek
            var liste = Models.VeriYoneticisi.VerileriGetir();

            // 2. Kullanıcıyı Bul
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici != null)
            {
                // 3. Siparişi Ekle (Listeyi başlatmadıysak null olabilir, kontrol et)
                if (aktifKullanici.Siparisler == null) aktifKullanici.Siparisler = new List<Models.Siparis>();

                aktifKullanici.Siparisler.Insert(0, yeniSiparis); // En başa ekle

                // 4. Kaydet
                Models.VeriYoneticisi.VerileriKaydet(liste);

                // Ayrıca bir de "Siparişiniz Alındı" mesajı atalım mı? :)
                if (aktifKullanici.Mesajlar == null) aktifKullanici.Mesajlar = new List<Models.Mesaj>();
                aktifKullanici.Mesajlar.Insert(0, new Models.Mesaj
                {
                    Baslik = "Sipariş Alındı",
                    Icerik = $"#{yeniSiparis.SiparisNo} nolu siparişiniz hazırlanıyor.",
                    Zaman = "Şimdi",
                    RenkClass = "bg-warning",
                    ResimYolu = "fas fa-box"
                });
                Models.VeriYoneticisi.VerileriKaydet(liste); // Tekrar kaydet (Mesaj için)

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult FavoriIslemi(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return Json(new { success = false, message = "Lütfen giriş yapınız." });

            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici != null)
            {
                // Liste null ise başlat
                if (aktifKullanici.FavoriUrunIdleri == null)
                    aktifKullanici.FavoriUrunIdleri = new List<int>();

                string islem = "";

                // VARSA SİL, YOKSA EKLE (Toggle)
                if (aktifKullanici.FavoriUrunIdleri.Contains(id))
                {
                    aktifKullanici.FavoriUrunIdleri.Remove(id);
                    islem = "cikarildi";
                }
                else
                {
                    aktifKullanici.FavoriUrunIdleri.Add(id);
                    islem = "eklendi";
                }

                Models.VeriYoneticisi.VerileriKaydet(liste);
                return Json(new { success = true, islem = islem });
            }

            return Json(new { success = false });
        }



        // 1. TEK SİPARİŞ SİLME
        public ActionResult SiparisSil(string siparisNo)
        {
            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici != null && aktifKullanici.Siparisler != null)
            {
                // Sipariş numarasından o siparişi bul ve sil
                var silinecek = aktifKullanici.Siparisler.FirstOrDefault(x => x.SiparisNo == siparisNo);
                if (silinecek != null)
                {
                    aktifKullanici.Siparisler.Remove(silinecek);
                    Models.VeriYoneticisi.VerileriKaydet(liste);
                }
            }
            // Sayfayı yenile
            return RedirectToAction("Gecmis_Siparislerim");
        }

        // 2. TÜM GEÇMİŞİ TEMİZLEME
        public ActionResult GecmisiTemizle()
        {
            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            if (aktifKullanici != null)
            {
                // Listeyi tamamen boşalt (Null yapma, new List yap)
                aktifKullanici.Siparisler = new List<Models.Siparis>();
                Models.VeriYoneticisi.VerileriKaydet(liste);
            }

            return RedirectToAction("Gecmis_Siparislerim");
        }

        // MESAJ SİLME METODU
        public ActionResult MesajSil(int id)
        {
            // 1. Listeyi Çek
            var liste = Models.VeriYoneticisi.VerileriGetir();
            var aktifKullanici = liste.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            // 2. Kullanıcıyı ve Mesajı Bul
            if (aktifKullanici != null && aktifKullanici.Mesajlar != null)
            {
                var silinecek = aktifKullanici.Mesajlar.FirstOrDefault(x => x.Id == id);

                // 3. Varsa Sil ve Kaydet
                if (silinecek != null)
                {
                    aktifKullanici.Mesajlar.Remove(silinecek);
                    Models.VeriYoneticisi.VerileriKaydet(liste);
                }
            }

            // 4. Mesaj Kutusuna Geri Dön
            return RedirectToAction("Mesajlarim");
        }

    }  // <-- HomeController sınıfı burada biter.
} // <-- Namespace burada biter.