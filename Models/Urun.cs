using System.Collections.Generic;

namespace ElektronikSatisProje.Models
{
    // Burası static OLMAMALI. Bu bir kalıptır.
    public class Urun
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public decimal Fiyat { get; set; }
        public string ResimYolu { get; set; }
        public string Rozet { get; set; }
        public string RozetClass { get; set; }
        public string Kampanya { get; set; }

        // --- Ürün Özellikleri için Eklenen Kısım ---
        public string Aciklama { get; set; } // Uzun yazı
        public string Kategori { get; set; }  // Ürün ekleme formunda kategori seçimi için
        public Dictionary<string, string> TeknikOzellikler { get; set; } // Tablo için (Örn: İşlemci -> i5)
    }
}