using ElektronikSatisProje.Models; // Modelleri tanıması için
using System.Linq;
using System.Web.Mvc;

namespace ElektronikSatisProje.Controllers
{
    public class AdminController : Controller
    {
        // YETKİ KONTROLÜ (Helper Metot)
        private bool IsUserAdmin()
        {
            if (!User.Identity.IsAuthenticated) return false;

            var kullanicilar = VeriYoneticisi.VerileriGetir();
            var aktifKullanici = kullanicilar.FirstOrDefault(x => x.KullaniciAdi == User.Identity.Name);

            return aktifKullanici != null && aktifKullanici.IsAdmin;
        }

        // 1. ÜRÜN LİSTESİ (PANEL ANA SAYFASI)
        public ActionResult Index()
        {
            if (!IsUserAdmin()) return RedirectToAction("Index", "Home"); // Yetkisiz giriş engeli

            var urunler = UrunDeposu.UrunleriGetir();
            return View(urunler);
        }

        // 2. YENİ ÜRÜN EKLEME SAYFASI (GET)
        public ActionResult Create()
        {
            if (!IsUserAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        // 3. YENİ ÜRÜNÜ KAYDETME (POST)
        [HttpPost]
        public ActionResult Create(Urun yeniUrun, System.Web.HttpPostedFileBase ResimDosyasi, string[] OzellikAdlari, string[] OzellikDegerleri)
        {
            if (!IsUserAdmin()) return RedirectToAction("Index", "Home");

            var urunler = UrunDeposu.UrunleriGetir();

            // 1. ID OLUŞTURMA
            int yeniId = urunler.Count > 0 ? urunler.Max(x => x.Id) + 1 : 1;
            yeniUrun.Id = yeniId;

            // 2. RESİM YÜKLEME
            if (ResimDosyasi != null && ResimDosyasi.ContentLength > 0)
            {
                string dosyaAdi = System.IO.Path.GetFileName(ResimDosyasi.FileName);
                string yol = Server.MapPath("~/Content/image/" + dosyaAdi);
                ResimDosyasi.SaveAs(yol);
                yeniUrun.ResimYolu = "/Content/image/" + dosyaAdi;
            }
            else
            {
                yeniUrun.ResimYolu = "/Content/image/default.jpg";
            }

            // 3. TEKNİK ÖZELLİKLERİ BİRLEŞTİRME (DİNAMİK GİRİŞ)
            yeniUrun.TeknikOzellikler = new System.Collections.Generic.Dictionary<string, string>();

            // Formdan gelen dizileri (Array) kontrol et
            if (OzellikAdlari != null && OzellikDegerleri != null)
            {
                // Dizilerde dön ve sözlüğe ekle
                for (int i = 0; i < OzellikAdlari.Length; i++)
                {
                    // Boş satırları atla
                    if (!string.IsNullOrWhiteSpace(OzellikAdlari[i]) && !string.IsNullOrWhiteSpace(OzellikDegerleri[i]))
                    {
                        // Aynı özellik adını (Key) iki kere eklemeyi engellemek için kontrol
                        string anahtar = OzellikAdlari[i].Trim();
                        string deger = OzellikDegerleri[i].Trim();

                        if (!yeniUrun.TeknikOzellikler.ContainsKey(anahtar))
                        {
                            yeniUrun.TeknikOzellikler.Add(anahtar, deger);
                        }
                    }
                }
            }

            // Eğer hiç özellik girilmediyse varsayılan bir tane ekle
            if (yeniUrun.TeknikOzellikler.Count == 0)
            {
                yeniUrun.TeknikOzellikler.Add("Durum", "Standart Ürün");
            }

            // Rozet Rengi (Varsayılan)
            yeniUrun.RozetClass = "badge-primary";

            // 4. KAYDET VE BİTİR
            urunler.Add(yeniUrun);
            UrunDeposu.UrunleriKaydet(urunler);

            return RedirectToAction("Index");
        }

        // 4. ÜRÜN SİLME
        public ActionResult Delete(int id)
        {
            if (!IsUserAdmin()) return RedirectToAction("Index", "Home");

            var urunler = UrunDeposu.UrunleriGetir();
            var silinecek = urunler.FirstOrDefault(x => x.Id == id);

            if (silinecek != null)
            {
                urunler.Remove(silinecek);
                UrunDeposu.UrunleriKaydet(urunler);
            }

            return RedirectToAction("Index");
        }

        // 5. ÜRÜN DÜZENLEME SAYFASI (GET) - Verileri Getirir
        public ActionResult Edit(int id)
        {
            if (!IsUserAdmin()) return RedirectToAction("Index", "Home");

            // Tüm ürünleri çek ve düzenlenen ürünü bul
            var urunler = UrunDeposu.UrunleriGetir();
            var duzenlenecekUrun = urunler.FirstOrDefault(x => x.Id == id);

            if (duzenlenecekUrun == null) return RedirectToAction("Index");

            return View(duzenlenecekUrun);
        }

        // 6. ÜRÜN GÜNCELLEME İŞLEMİ (POST) - Verileri Kaydeder
        [HttpPost]
        public ActionResult Edit(Urun gelenUrun, string[] OzellikAdlari, string[] OzellikDegerleri)
        {
            if (!IsUserAdmin()) return RedirectToAction("Index", "Home");

            var urunler = UrunDeposu.UrunleriGetir();
            var mevcutUrun = urunler.FirstOrDefault(x => x.Id == gelenUrun.Id);

            if (mevcutUrun != null)
            {
                // 1. Temel Bilgileri Güncelle
                mevcutUrun.Ad = gelenUrun.Ad;
                mevcutUrun.Fiyat = gelenUrun.Fiyat;
                mevcutUrun.Rozet = gelenUrun.Rozet;
                mevcutUrun.Aciklama = gelenUrun.Aciklama;

                // (Resim güncelleme şimdilik yok, eskisi kalsın)
                // mevcutUrun.ResimYolu = ... 

                // 2. TEKNİK ÖZELLİKLERİ SİL VE YENİDEN OLUŞTUR
                // Eski listeyi tamamen temizliyoruz, formdan gelen yeni halini yazacağız.
                mevcutUrun.TeknikOzellikler = new System.Collections.Generic.Dictionary<string, string>();

                if (OzellikAdlari != null && OzellikDegerleri != null)
                {
                    for (int i = 0; i < OzellikAdlari.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(OzellikAdlari[i]) && !string.IsNullOrWhiteSpace(OzellikDegerleri[i]))
                        {
                            string anahtar = OzellikAdlari[i].Trim();
                            string deger = OzellikDegerleri[i].Trim();

                            if (!mevcutUrun.TeknikOzellikler.ContainsKey(anahtar))
                            {
                                mevcutUrun.TeknikOzellikler.Add(anahtar, deger);
                            }
                        }
                    }
                }

                // Eğer boşsa varsayılan atama
                if (mevcutUrun.TeknikOzellikler.Count == 0)
                {
                    mevcutUrun.TeknikOzellikler.Add("Durum", "Standart");
                }

                // 3. KAYDET
                UrunDeposu.UrunleriKaydet(urunler);
            }

            return RedirectToAction("Index");
        }
    }
}