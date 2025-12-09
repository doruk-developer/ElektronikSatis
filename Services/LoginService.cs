using ElektronikSatisProje.Interfaces;
using ElektronikSatisProje.Models.DTO.Login;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElektronikSatisProje.Services
{
    public class LoginService : ILoginService
    {
        public KayitOlResponse EmailDogrula(string email)
        {
            var izinverilenEmailler = new List<string>
            {
                "mail1@gmail.com",
                "mail2@gmail.com",
                "mail3@gmail.com",
                "mail4@gmail.com",
                "mail5@gmail.com",
                "mail6@gmail.com",
                "mail7@gmail.com",
                "mail8@gmail.com",
                "mail9@gmail.com",
                "mail10@gmail.com"
            };

            if (izinverilenEmailler.Contains(email, StringComparer.OrdinalIgnoreCase))
            {
                return new KayitOlResponse
                {
                    IsSuccess = true,
                    Message = "Kayıt başarı ile oluşturuldu."
                };
            }

            return new KayitOlResponse
            {
                IsSuccess = false,
                Message = "Bu email adresi kayıt için yetkili değil"
            };
        }
    }
}