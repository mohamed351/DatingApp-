using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace DatingApp.API.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;

        }
        public async Task<bool> IsExist(string userName)
        {
          var user =  await this.context.User.FirstOrDefaultAsync(a => a.UserName == userName);
            return user == null ? false : true;
        }

        public async Task<User> Login(string userName, string password)
        {
           var user =  await this.context.User.FirstOrDefaultAsync(a => a.UserName == userName);
           if(user == null){
                return null;
            }
            else{
               if(await VerifyPasswordHashing(password, user.PasswordHashed, user.PasswordSalt))
                    return user;
                else
                    return null;
                
            }

        }

        private async Task<bool> VerifyPasswordHashing(string password, byte[] passwordHashed,  byte[] passwordSalt)
        {
            using(var hasing = new HMACSHA512(passwordSalt)){

                var newPasswordHashing = hasing.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < newPasswordHashing.Length; i++)
                {
                    if(newPasswordHashing[i] != passwordHashed[i]) return false;
                }
            }
            return true;

        }

        public async Task<User> Registration(string userName, string email, string name, string password)
        {
            byte[] passwordHashing = null;
            byte[] passwordSalting = null;
            PasswordHashing(password, ref passwordHashing, ref passwordSalting);
            var user = new User()
            {
                Email = email,
                Name = name,
                UserName = userName,
                PasswordHashed = passwordHashing,
                PasswordSalt = passwordSalting,
            
            };
            await context.AddAsync(user);
            await context.SaveChangesAsync();
            return user;

        }

        private void PasswordHashing(string passsword, ref byte[] passwordHashing, ref byte[] passwordSalting)
        {
            using(var hasing = new HMACSHA512()){

                passwordHashing = hasing.ComputeHash(Encoding.UTF8.GetBytes(passsword));
                passwordSalting = hasing.Key;
            }
        }
    }
}