using ElektronikSatisProje.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace ElektronikSatisProje.Controllers
{
    public class LoginController : Controller
    {
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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CheckLogin(string name, string password)
        {
            // 1. Dosyadan kayıtlı veriyi çek
            var kayitliVeri = Models.VeriYoneticisi.VeriyiGetir();

            // 2. Dosyadaki kullanıcı adı ve şifreyle karşılaştır
            if (name == kayitliVeri.KullaniciAdi && password == kayitliVeri.Sifre)
            {
                FormsAuthentication.SetAuthCookie(name, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.HataMesaji = "Kullanıcı adı veya şifre hatalı.";
                return View("Index");
            }
        } // <-- CheckLogin METODU BURADA BİTER.

        // ==========================================================
        // ===== ŞİFREMİ UNUTTUM METOTLARI ARTIK DOĞRU YERDE =====
        // ==========================================================

        // 1. Şifremi Unuttum sayfasını GÖSTERMEK için bu metot çalışır.
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // 2. Formdaki "Şifremi Sıfırla" butonuna basıldığında BU METOT çalışır.
        [HttpPost]
        public ActionResult ForgotPassword(string NewPassword, string ConfirmPassword)
        {
            // --- BU BÖLÜM SİMÜLASYONDUR ---
            // Gerçek bir projede, burada veritabanından Email'e sahip kullanıcı bulunur.
            // Biz şimdilik sizin sabit "Doruk" kullanıcısını baz alacağız.
            // Ve ona bir e-posta adresi atayacağız.
            string hardcodedUserEmail = "doruk@mail.com";

            
            if (NewPassword != ConfirmPassword)
            {
                // Şifreler uyuşmuyorsa, aynı sayfada kalıp hata göster.
                TempData["ErrorMessage"] = "Girilen yeni şifreler eşleşmiyor.";
                return View();
            }

            // --- ŞİFRE GÜNCELLEME SİMÜLASYONU ---
            // Her şey yolundaysa, şifrenin güncellendiğini varsayalım.
            // Gerçek bir projede bu kısımda veritabanı güncelleme kodu olur.
            // Örneğin: user.Password = newHashedPassword; db.SaveChanges();

            // Başarılı mesajı göster ve kullanıcıyı giriş sayfasına yönlendir.
            TempData["SuccessMessage"] = "Şifreniz başarıyla güncellendi. Lütfen giriş yapın.";
            return RedirectToAction("Index", "Login");
        }

    } // <-- LoginController SINIFI (CLASS) BURADA BİTER.
}