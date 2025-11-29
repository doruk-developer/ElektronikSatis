using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}