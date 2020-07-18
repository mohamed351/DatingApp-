using System.Threading.Tasks;
using DatingApp.API.Models;
namespace DatingApp.API.Repository
{
    public interface IAuthRepository
    {
        Task<User> Registration(string userName, string email, string name, string password);
        Task<User> Login(string userName, string password);

        Task<bool> IsExist(string userName);

    }
}