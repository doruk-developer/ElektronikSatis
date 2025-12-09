namespace ElektronikSatisProje.Models
{
    public class Profil
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Sehir { get; set; }
        public string ResimYolu { get; set; }
        public int ToplamSiparis { get; set; } // İstatistik için
        public string DogumTarihi { get; set; }
        public System.Collections.Generic.List<Adres> Adresler { get; set; } = new System.Collections.Generic.List<Adres>(); // Adres listesi
    }
}