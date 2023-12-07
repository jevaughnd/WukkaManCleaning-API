using WukkaManCleaning_API.Models;

namespace WukkaManCleaning_API.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
        Task<bool> Login(User user);
        Task<bool> RegisterUser(User user);
    }
}