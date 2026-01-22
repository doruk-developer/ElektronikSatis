using ElektronikSatis.Models.DTO.Login;

namespace ElektronikSatis.Interfaces
{
    public interface ILoginService
    {
        KayitOlResponse EmailDogrula(string email);
    }
}
