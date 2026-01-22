using ElektronikSatis.Interfaces;
using ElektronikSatis.Models.DTO.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace ElektronikSatis.Controllers
{
    public class LoginController : Controller
    {
        private ILoginService _loginSrevice;

        public LoginController(ILoginService loginService)
        {
            _loginSrevice = loginService;
        }

        // GET: Login
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Logout()
        {
            // Kullanıcının oturumunu sonlandırır
            FormsAuthentication.SignOut();

            // Kullanıcıyı tekrar giriş sayfasına yönlendirir
            return RedirectToAction("Index", "Login");
        }

        //public ActionResult Index(string message)
        //{
        //    return View(new ErrorDTO { Message = message });
        //}



        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckLogin(string name, string password)
        {
            // 1. Veri Yöneticisinden listeyi çek
            var kullaniciListesi = Models.VeriYoneticisi.VerileriGetir();

            // 2. Kontrol et (Kullanıcı Adı VE Şifre eşleşiyor mu?)
            var girisYapan = kullaniciListesi.FirstOrDefault(x => x.KullaniciAdi == name && x.Sifre == password);

            if (girisYapan != null)
            {
                // --- BAŞARILI ---
                // Bileti Kes (Cookie)
                System.Web.Security.FormsAuthentication.SetAuthCookie(name, false);

                // JS'e "Tamamdır, yönlenebilirsin" de
                return Json(new { success = true });
            }
            else
            {
                // --- HATALI ---
                // İsteğine özel hata mesajı (HTML formatında gönderiyoruz ki kalın yapabilelim)
                string mesaj = "Kullanıcı adı veya şifre hatalı.<br><b>Lütfen bilgilerinizi kontrol ediniz.</b>";

                return Json(new { success = false, message = mesaj });
            }
        } // <-- CheckLogin METODU BURADA BİTER.

        // ==========================================================
        // ===== ŞİFREMİ UNUTTUM METOTLARI ARTIK DOĞRU YERDE =====
        // ==========================================================

        // 1. Şifremi Unuttum sayfasını GÖSTERMEK için bu metot çalışır.
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult KayitOL(KayitOlRequest request)
        {
            // 1. Mail yetkili mi kontrol et (LoginService)
            var response = _loginSrevice.EmailDogrula(request.Email);

            if (response.IsSuccess)
            {
                // 2. MAİL YETKİLİYSE -> SİSTEME KAYDET ve Bir "Hoşgeldin" mesajı da at.

                var yeniKullanici = new Models.KullaniciVerisi // (VeriYoneticisi.KullaniciVerisi değil, direkt KullaniciVerisi)
                {
                    KullaniciAdi = request.Username,
                    Sifre = request.Password,
                    ProfilBilgisi = new Models.Profil
                    {
                        Email = request.Email,
                        Ad = request.Username,
                        Soyad = "", // Başlangıçta boş
                        Telefon = "",
                        Sehir = "",
                        ResimYolu = "/Content/image/user_circle.png"
                    },

                    // --- BURASI YENİ: OTOMATİK HOŞ GELDİN MESAJI ---
                    Mesajlar = new List<Models.Mesaj>
                {
                    new Models.Mesaj
                    {
                        Baslik = "Aramıza Hoş Geldin!",
                        Icerik = "Üyeliğiniz başarıyla oluşturuldu. Keyifli alışverişler dileriz.",
                        Zaman = "Şimdi",
                        RenkClass = "bg-success", // Yeşil ikon
                        ResimYolu = "fas fa-smile"
                    },
                    new Models.Mesaj
                    {
                        Baslik = "İlk İpucu",
                        Icerik = "Profil sayfasından bilgilerinizi güncellemeyi unutmayın.",
                        Zaman = "Şimdi",
                        RenkClass = "bg-primary", // Mavi ikon
                        ResimYolu = "fas fa-info"
                    }
                }
                    // -----------------------------------------------
                };

                Models.VeriYoneticisi.KullaniciEkle(yeniKullanici);
            }

            // 3. Cevabı (Başarılı/Başarısız) View'a döndür
            return Json(response);
        }

        // 2. Formdaki "Şifremi Sıfırla" butonuna basıldığında BU METOT çalışır.
        // Şifremi Unuttum İşlemi (Simülasyon)
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(string Email, string Username)
        {
            // 1. Mevcut listeyi çek
            var liste = Models.VeriYoneticisi.VerileriGetir();

            // 2. Eşleşme Kontrolü
            var kullanici = liste.FirstOrDefault(x =>
                x.KullaniciAdi.Equals(Username, StringComparison.OrdinalIgnoreCase) &&
                x.ProfilBilgisi.Email.Equals(Email, StringComparison.OrdinalIgnoreCase)
            );

            if (kullanici == null)
            {
                return Json(new { success = false, message = "Bu bilgilere ait bir kayıt bulunamadı." });
            }

            // 3. Yeni Şifre Üret
            string yeniSifre = Guid.NewGuid().ToString().Substring(0, 6);

            // 4. Veritabanını Güncelle
            kullanici.Sifre = yeniSifre;
            Models.VeriYoneticisi.VerileriKaydet(liste);

            // ============================================================
            // --- SMS SİMÜLASYONU (ÜZERİNE YAZMA MODU) ---
            // ============================================================
            try
            {
                // Kullanıcının telefonunu al (Boşsa uyarı yaz)
                string telefonNo = string.IsNullOrEmpty(kullanici.ProfilBilgisi.Telefon)
                                   ? "Kayıtlı Telefon Yok"
                                   : kullanici.ProfilBilgisi.Telefon;

                // İstenilen formatta mesaj içeriği
                string mesajIcerigi = $"Bu SMS şu numaralı telefona gitti ---> {telefonNo}\n" +
                                      $"--------------------------------------------------\n" +
                                      $"TARİH: {DateTime.Now}\n" +
                                      $"KİME: {Username} ({Email})\n" +
                                      $"KONU: Şifre Sıfırlama Talebi\n" +
                                      $"\n" +
                                      $"Sayın {Username},\n" +
                                      $"Hesabınızın şifresi başarıyla sıfırlanmıştır.\n\n" +
                                      $"YENİ ŞİFRENİZ: {yeniSifre}\n\n" +
                                      $"Güvenliğiniz için giriş yaptıktan sonra şifrenizi değiştiriniz.\n" +
                                      $"Doruk Shop Güvenlik Ekibi";

                // Klasör Yolu
                string klasorYolu = Server.MapPath("~/App_Data/GonderilenSMSler");

                if (!System.IO.Directory.Exists(klasorYolu))
                {
                    System.IO.Directory.CreateDirectory(klasorYolu);
                }

                // Dosya Adı SABİT (Böylece her seferinde eskisi silinip yenisi yazılır)
                string sabitDosyaAdi = "SonGonderilenSMS.txt";
                string tamYol = System.IO.Path.Combine(klasorYolu, sabitDosyaAdi);

                // Dosyayı Yaz (WriteAllText metodu varsa üzerine yazar, yoksa oluşturur)
                System.IO.File.WriteAllText(tamYol, mesajIcerigi);
            }
            catch (Exception)
            {
                // Hata olursa sistemi bozma
            }
            // ============================================================

            // 5. Ekrana da göster
            return Json(new { success = true, newPass = yeniSifre });
        } // <-- ForgotPassword ActionResult'ının bitimi.

    } // <-- LoginController SINIFI (CLASS) BURADA BİTER.
}