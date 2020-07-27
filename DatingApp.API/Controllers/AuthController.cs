using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DatingApp.API.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;
        private readonly IConfiguration configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.authRepository = authRepository;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Registraion(RegistraionDTO dto)
        {
            if (await this.authRepository.IsExist(dto.UserName))
                return BadRequest("The User is Exist");
            var User = await this.authRepository.Registration(dto.UserName, dto.Email, dto.Name, dto.Password);
            JwtSecurityTokenHandler handler;
            SecurityToken token;
            JwtGenerator(User, out handler, out token);
            return Ok(new
            {
                Token = handler.WriteToken(token),
                ExpireDate = token.ValidTo

            });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await this.authRepository.Login(dto.UserName, dto.Password);
            if (user == null)
                return Unauthorized();

            JwtSecurityTokenHandler handler;
            SecurityToken token;
            JwtGenerator(user, out handler, out token);

            return Ok(new
            {
                Token = handler.WriteToken(token),
                ExpireDate = token.ValidTo

            });



        }

        private void JwtGenerator(Models.User user, out JwtSecurityTokenHandler handler, out SecurityToken token)
        {
            var claims = new Claim[]{
                new Claim(ClaimTypes.NameIdentifier,user.ID),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Surname , user.Name)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var signIncreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var descrptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signIncreds,
                Expires = DateTime.Now.AddDays(2)
            };
            handler = new JwtSecurityTokenHandler();
            token = handler.CreateToken(descrptor);
        }
    }
}