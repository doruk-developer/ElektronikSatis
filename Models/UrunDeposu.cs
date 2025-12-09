using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Web;
// VEYA System.Web.Script.Serialization kullanabilirsin (önceki gibi)

namespace ElektronikSatisProje.Models
{
    public static class UrunDeposu
    {
        private static string DosyaYolu = HttpContext.Current.Server.MapPath("~/App_Data/urunler.json");

        // Ürünleri Getir (READ)
        public static List<Urun> UrunleriGetir()
        {
            if (!File.Exists(DosyaYolu)) return new List<Urun>();

            string json = File.ReadAllText(DosyaYolu);

            // System.Web.Script.Serialization kullanarak:
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Deserialize<List<Urun>>(json);
        }

        // Ürün Kaydet (CREATE / UPDATE / DELETE)
        // (Bunu Admin paneli için hazırladık)
        // --- Raflı JSON için Böyle ---
        public static void UrunleriKaydet(List<Urun> urunler)
        {
            // Formatting.Indented -> İşte veriyi "Raflı/Alt Alta" yapan sihirli komut budur.
            string json = JsonConvert.SerializeObject(urunler, Formatting.Indented);

            File.WriteAllText(DosyaYolu, json);
        }
    }
}