using System.Collections.Generic;

namespace ElektronikSatis.Models
{
    public static class MesajDeposu
    {
        public static List<Mesaj> MesajlariGetir()
        {
            return new List<Mesaj>
            {
                new Mesaj {
                    Id = 1, // ID EKLENDİ
                    Baslik = "Hoş Geldin Hediyesi!",
                    Icerik = "İlk alışverişine özel %10 indirim kuponun hesabına tanımlandı.",
                    Zaman = "Az önce",
                    ResimYolu = "fas fa-gift",
                    RenkClass = "bg-primary"
                },
                new Mesaj {
                    Id = 2, // ID EKLENDİ
                    Baslik = "Kargo Bedava Fırsatı",
                    Icerik = "Bugün yapacağın 500 TL üzeri alışverişlerde kargo bizden!",
                    Zaman = "2 saat önce",
                    ResimYolu = "fas fa-truck",
                    RenkClass = "bg-success"
                },
                new Mesaj {
                    Id = 3, // ID EKLENDİ
                    Baslik = "Fiyatı Düştü!",
                    Icerik = "Favori listendeki Xiaomi Redmi Note 13 indirime girdi. Kaçırma!",
                    Zaman = "1 gün önce",
                    ResimYolu = "fas fa-tag",
                    RenkClass = "bg-warning"
                },
                new Mesaj {
                    Id = 4, // ID EKLENDİ
                    Baslik = "Siparişin Yola Çıktı",
                    Icerik = "#12345 numaralı siparişin kargoya verildi.",
                    Zaman = "2 gün önce",
                    ResimYolu = "fas fa-box-open",
                    RenkClass = "bg-info"
                }
            };
        }
    }
}