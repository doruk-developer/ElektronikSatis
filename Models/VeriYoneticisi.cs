using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;

namespace ElektronikSatis.Models
{
    // 1. KULLANICI VERİSİ MODELİ (Artık dışarıda ve herkes erişebilir)
    // 1. KULLANICI VERİSİ MODELİ
    public class KullaniciVerisi
    {
        public string KullaniciAdi { get; set; } = "Doruk";
        public string Sifre { get; set; } = "şifre";
        public Models.Profil ProfilBilgisi { get; set; }

        // --- YENİ EKLENEN: KİŞİSEL MESAJ KUTUSU ---
        public List<Models.Mesaj> Mesajlar { get; set; } = new List<Models.Mesaj>();

        // --- YENİ EKLENEN: SİPARİŞ GEÇMİŞİ(Kişiye özel siparişleri göstermek için) ---
        // Not: Sipariş sınıfın yoksa Models klasörüne Siparis.cs eklemelisin
        public List<Models.Siparis> Siparisler { get; set; } = new List<Models.Siparis>();

        // --- YENİ EKLENEN: FAVORİ ID LİSTESİ ---
        public List<int> FavoriUrunIdleri { get; set; } = new List<int>();

        // YENİ: Girişe gizli admin kontrolü
        public bool IsAdmin { get; set; } = false;
    }

    // 2. VERİ YÖNETİCİSİ (Veritabanı İşlemleri)
    public static class VeriYoneticisi
    {
        // Dosya yolu: App_Data klasörü
        private static string DosyaYolu = HttpContext.Current.Server.MapPath("~/App_Data/kullanici_listesi.json");

        // --- OKUMA İŞLEMİ (LİSTE DÖNER) ---
        public static List<KullaniciVerisi> VerileriGetir()
        {
            if (!File.Exists(DosyaYolu))
            {
                // Dosya yoksa varsayılan kullanıcıyla oluştur
                var varsayilanListe = new List<KullaniciVerisi>();
                varsayilanListe.Add(new KullaniciVerisi
                {
                    KullaniciAdi = "Doruk",
                    Sifre = "şifre",
                    ProfilBilgisi = new Models.Profil
                    {
                        Ad = "Doruk",
                        Soyad = "Yılmaz",
                        Email = "mail1@google.com",
                        Telefon = "0555 123 45 67",
                        Sehir = "İstanbul",
                        ResimYolu = "/Content/image/user_circle.png"
                    }
                });

                VerileriKaydet(varsayilanListe);
                return varsayilanListe;
            }

            // Dosya varsa oku ve listeye çevir
            string jsonText = File.ReadAllText(DosyaYolu);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Deserialize<List<KullaniciVerisi>>(jsonText);
        }

        // --- KAYDETME İŞLEMİ (LİSTEYİ YAZAR) ---
        public static void VerileriKaydet(List<KullaniciVerisi> liste)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string jsonText = serializer.Serialize(liste);
            File.WriteAllText(DosyaYolu, jsonText);
        }

        // --- TEK KULLANICI EKLEME YARDIMCISI ---
        public static void KullaniciEkle(KullaniciVerisi yeniKullanici)
        {
            var liste = VerileriGetir();
            liste.Add(yeniKullanici);
            VerileriKaydet(liste);
        }
    }
}