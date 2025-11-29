using System.Collections.Generic;

namespace ElektronikSatisProje.Models
{
    public static class UrunDeposu
    {
        public static List<Urun> UrunleriGetir()
        {
            return new List<Urun>
            {
                // 1. Bilgisayar
                new Urun {
                    Id = 1,
                    Ad = "Asus Vivobook 15",
                    Fiyat = 18499,
                    ResimYolu = "/Content/image/laptop1.jpg",
                    Rozet = "Çok Satan",
                    RozetClass = "badge-success",
                    Aciklama = "İnce ve hafif tasarımıyla Asus Vivobook, günlük işlerinizde size hız kazandırır.",
                    TeknikOzellikler = new Dictionary<string, string> { { "İşlemci", "Intel i5" }, { "RAM", "8GB" }, { "SSD", "512GB" } }
                },

                // 2. Bilgisayar
                new Urun {
                    Id = 2,
                    Ad = "MSI Thin 15",
                    Fiyat = 31898,
                    ResimYolu = "/Content/image/laptop2.jpg",
                    Rozet = "Fırsat",
                    RozetClass = "badge-warning",
                    Aciklama = "Oyun dünyasına güçlü bir giriş yapın. Yüksek soğutma performansı.",
                    TeknikOzellikler = new Dictionary<string, string> { { "İşlemci", "Intel i5-H" }, { "RAM", "16GB" }, { "Ekran Kartı", "RTX 4050" } }
                },

                // 3. Telefon (İŞTE BU EKSİKTİ MUHTEMELEN)
                new Urun {
                    Id = 3,
                    Ad = "Xiaomi Redmi Note 13",
                    Fiyat = 16499,
                    ResimYolu = "/Content/image/telefon1.png",
                    Rozet = "Popüler",
                    RozetClass = "badge-primary",
                    Aciklama = "200MP kamerası ile her anı profesyonelce yakalayın.",
                    TeknikOzellikler = new Dictionary<string, string> { { "Kamera", "200MP" }, { "Batarya", "5000mAh" }, { "Ekran", "AMOLED" } }
                },

                // 4. Telefon
                new Urun {
                    Id = 4,
                    Ad = "General Mobile GM 24",
                    Fiyat = 9999,
                    ResimYolu = "/Content/image/telefon2.png",
                    Rozet = "Ekonomik",
                    RozetClass = "badge-info",
                    Aciklama = "Yerli üretim gücü, şık tasarım ve uygun fiyat.",
                    TeknikOzellikler = new Dictionary<string, string> { { "Hafıza", "128GB" }, { "RAM", "8GB" } }
                },

                // 5. Network (Switch)
                new Urun {
                    Id = 5,
                    Ad = "Cisco Catalyst 9200L",
                    Fiyat = 34500,
                    ResimYolu = "/Content/image/switch1.jpg",
                    Rozet = "Pro",
                    RozetClass = "badge-dark",
                    Aciklama = "Kurumsal ağlar için güvenlik ve hız bir arada.",
                    TeknikOzellikler = new Dictionary<string, string> { { "Port", "24x PoE+" }, { "Layer", "L3" } }
                },

                // 6. Network (Switch)
                new Urun {
                    Id = 6,
                    Ad = "Huawei CloudEngine",
                    Fiyat = 15750,
                    ResimYolu = "/Content/image/switch2.jpg",
                    Rozet = "Yeni",
                    RozetClass = "badge-success",
                    Aciklama = "Akıllı yönetim özellikleriyle bulut tabanlı switch.",
                    TeknikOzellikler = new Dictionary<string, string> { { "Port", "24x Gigabit" }, { "Yönetim", "Cloud" } }
                },

                // 7. Network (Router)
                new Urun {
                    Id = 7,
                    Ad = "Cisco ISR 1100",
                    Fiyat = 22499,
                    ResimYolu = "/Content/image/router1.jpg",
                    Rozet = "Hızlı",
                    RozetClass = "badge-danger",
                    Aciklama = "Şubeler arası güvenli bağlantı için endüstri standardı.",
                    TeknikOzellikler = new Dictionary<string, string> { { "WAN", "1 Gbps" }, { "SD-WAN", "Destekler" } }
                },

                // 8. Network (Router)
                new Urun {
                    Id = 8,
                    Ad = "Huawei NetEngine",
                    Fiyat = 12850,
                    ResimYolu = "/Content/image/router2.jpg",
                    Rozet = "İndirim",
                    RozetClass = "badge-warning",
                    Aciklama = "Kompakt ve güçlü router çözümü.",
                    TeknikOzellikler = new Dictionary<string, string> { { "VPN", "Var" }, { "Firewall", "Entegre" } }
                }
            };
        }
    }
}