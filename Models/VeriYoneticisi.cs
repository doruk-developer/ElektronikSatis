using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization; // JSON işlemleri için (System.Web.Extensions referansı gerekebilir)

namespace ElektronikSatisProje.Models
{
    // Bu sınıf bizim veritabanımız gibi davranacak
    public static class VeriYoneticisi
    {
        // Dosyanın kaydedileceği yer (App_Data klasörü güvenlidir, dışarıdan erişilmez)
        private static string DosyaYolu = HttpContext.Current.Server.MapPath("~/App_Data/kullanici_ayarlari.json");

        // Kaydedilecek Veri Modeli
        public class KullaniciVerisi
        {
            public string KullaniciAdi { get; set; } = "Doruk";
            public string Sifre { get; set; } = "rICA"; // Varsayılan şifre
            public Models.Profil ProfilBilgisi { get; set; }
        }

        // VERİYİ OKU
        public static KullaniciVerisi VeriyiGetir()
        {
            if (!File.Exists(DosyaYolu))
            {
                // Eğer dosya henüz yoksa (ilk çalıştırış), varsayılan bir tane oluştur
                var varsayilan = new KullaniciVerisi
                {
                    ProfilBilgisi = new Models.Profil
                    {
                        Ad = "Doruk",
                        Soyad = "Yılmaz",
                        Email = "admin@dorukshop.com",
                        Telefon = "0555 123 45 67",
                        Sehir = "İstanbul",
                        ResimYolu = "/Content/image/user_circle.png"
                    }
                };
                VeriyiKaydet(varsayilan);
                return varsayilan;
            }

            // Dosya varsa oku ve C# nesnesine çevir
            string jsonText = File.ReadAllText(DosyaYolu);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<KullaniciVerisi>(jsonText);
        }

        // VERİYİ KAYDET
        public static void VeriyiKaydet(KullaniciVerisi veri)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonText = serializer.Serialize(veri);
            File.WriteAllText(DosyaYolu, jsonText);
        }
    }
}