using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElektronikSatisProje.Models
{
    public class Mesaj
    {
        public int Id { get; set; } // Tıklanan mesajın hangisi olduğunu, sisteme verebilmek için Id bilgisi göndereceğiz.
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string Zaman { get; set; } // Örn: "20 dk önce"
        public string ResimYolu { get; set; } // Kampanya ikonu için
        public string RenkClass { get; set; } // İkonun arka plan rengi (bg-primary vb.)
    }
}