using ElektronikSatisProje.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElektronikSatisProje.Controllers
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

        // 1. Profil verisini hafızada tutmak için static bir değişken tanımlıyoruz.
        // Böylece resim yüklenince sayfa yenilense bile yeni resim yolu burada kalır.
        public static Models.Profil MevcutProfil = new Models.Profil
        {
            Ad = "Doruk",
            Soyad = "Yılmaz",
            Email = "admin@dorukshop.com",
            Telefon = "0555 123 45 67",
            Sehir = "İstanbul",
            // Başlangıçta varsayılan resim
            ResimYolu = Models.VeriYoneticisi.VeriyiGetir().ProfilBilgisi.ResimYolu
        };

        public ActionResult Profil()
        {
            // Veriyi dosyadan oku ve View'a gönder
            var veri = Models.VeriYoneticisi.VeriyiGetir();
            return View(veri.ProfilBilgisi);
        }

        [HttpPost]
        public ActionResult UploadProfileImage()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = "custom_profile_doruk.jpg";
                    var path = System.IO.Path.Combine(Server.MapPath("~/Content/image"), fileName);
                    file.SaveAs(path);

                    // --- DEĞİŞEN KISIM BURASI ---
                    // 1. Mevcut veriyi dosyadan çek
                    var veri = Models.VeriYoneticisi.VeriyiGetir();

                    // 2. Resim yolunu güncelle (Browser cache için versiyon ekle)
                    string yeniYol = "/Content/image/" + fileName + "?v=" + DateTime.Now.Ticks;
                    veri.ProfilBilgisi.ResimYolu = yeniYol;

                    // 3. Veriyi tekrar dosyaya kaydet (KALICILIK İŞTE BURADA SAĞLANIYOR)
                    Models.VeriYoneticisi.VeriyiKaydet(veri);

                    return Json(new { success = true, newUrl = yeniYol });
                }
            }
            return Json(new { success = false, message = "Dosya yüklenemedi." });
        }

        [HttpPost]
        public ActionResult ChangePassword(string currentPass, string newPass)
        {
            var veri = Models.VeriYoneticisi.VeriyiGetir();

            // Eski şifre doğru mu?
            if (veri.Sifre != currentPass)
            {
                return Json(new { success = false, message = "Mevcut şifreniz hatalı!" });
            }

            // Şifreyi güncelle ve kaydet
            veri.Sifre = newPass;
            Models.VeriYoneticisi.VeriyiKaydet(veri);

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
                return View(new List<ElektronikSatisProje.Models.Urun>());
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
            // 1. Tüm ürünleri depodan getir
            var tumUrunler = UrunDeposu.UrunleriGetir();

            // 2. Gelen ID ile eşleşen ürünü bul (LINQ sorgusu)
            var secilenUrun = tumUrunler.FirstOrDefault(x => x.Id == id);

            // 3. Bulunan ürünü sayfaya gönder
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

        // MESAJ KUTUSU (Inbox)
        public ActionResult Mesajlarim()
        {
            var mesajlar = ElektronikSatisProje.Models.MesajDeposu.MesajlariGetir();
            return View(mesajlar);
        }

        // MESAJ OKUMA (Detail)
        public ActionResult MesajOku(int id)
        {
            var mesajlar = ElektronikSatisProje.Models.MesajDeposu.MesajlariGetir();
            var secilenMesaj = mesajlar.FirstOrDefault(x => x.Id == id);

            if (secilenMesaj == null) return RedirectToAction("Mesajlarim");

            return View(secilenMesaj);
        }

        public ActionResult Bilgisayarlar()
        {
            // 1. Tüm ürünleri depodan çek
            var tumUrunler = UrunDeposu.UrunleriGetir();

            // 2. Sadece Bilgisayarları (ID'si 1 ve 2 olanları) filtrele
            // (İleride "Kategori" alanı ekleyince daha kolay olacak, şimdilik ID ile yapıyoruz)
            var bilgisayarlar = tumUrunler.Where(x => x.Id == 1 || x.Id == 2).ToList();

            // 3. Listeyi sayfaya gönder
            return View(bilgisayarlar);
        }

        // 1. CEP TELEFONLARI (ID: 3 ve 4)
        public ActionResult Cep_Telefonları()
        {
            var tumUrunler = UrunDeposu.UrunleriGetir();
            // UrunDeposu'nda Telefonlara 3 ve 4 vermiştik
            var telefonlar = tumUrunler.Where(x => x.Id == 3 || x.Id == 4).ToList();
            return View(telefonlar);
        }

        public ActionResult Switchler()
        {
            var tumUrunler = UrunDeposu.UrunleriGetir();
            // UrunDeposu'nda Switchlere 5 ve 6 vermiştik
            var switchler = tumUrunler.Where(x => x.Id == 5 || x.Id == 6).ToList();
            return View(switchler);
        }

        public ActionResult Routerlar()
        {
            var tumUrunler = UrunDeposu.UrunleriGetir();
            // UrunDeposu'nda Routerlara 7 ve 8 vermiştik
            var routerlar = tumUrunler.Where(x => x.Id == 7 || x.Id == 8).ToList();
            return View(routerlar);
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

        public ActionResult Geçmiş_Siparişlerim()
        {
            return View();
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
            return View();
        }

        // GEÇMİŞ SİPARİŞLERİM (Zaten varsa içini böyle güncelle)
        public ActionResult Gecmis_Siparislerim()
        {
            return View();
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

        

    } // <-- HomeController SINIFI (CLASS) ARTIK BURADA, DOĞRU YERDE BİTER.

} // <-- Namespace burada biter.