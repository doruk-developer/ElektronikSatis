using ElektronikSatisProje.Models.DTO.Login;

namespace ElektronikSatisProje.Interfaces
{
    public interface ILoginService
    {
        KayitOlResponse EmailDogrula(string email);
    }
}
