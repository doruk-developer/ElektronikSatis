using System.Collections.Generic;

namespace ElektronikSatisProje.Models
{
    // Siparişin genel yapısı
    public class Siparis
    {
        public string SiparisNo { get; set; }
        public string Tarih { get; set; }
        public string ToplamTutar { get; set; }
        public string Durum { get; set; }
        public string TeslimatAdresi { get; set; } // Siparişin gittiği adres (METİN OLARAK)
        // Siparişin içindeki ürünlerin listesi
        public List<SepetUrunu> Urunler { get; set; }
    }

    // Siparişin içindeki tek bir ürünün yapısı
    public class SepetUrunu
    {
        public string UrunAdi { get; set; }
        public string Resim { get; set; }
        public decimal Fiyat { get; set; }
        public int Adet { get; set; }
    }
}