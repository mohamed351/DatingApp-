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
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await this.authRepository.Login(dto.UserName, dto.Password);
            if (user == null)
                return Unauthorized();


            var claims = new Claim[]{
                new Claim(ClaimTypes.NameIdentifier,user.ID),
                new Claim(ClaimTypes.Name,user.UserName)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value));
            var signIncreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var descrptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signIncreds,
                Expires = DateTime.Now.AddDays(2)
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descrptor);

            return Ok(new
            {
                Token = handler.WriteToken(token),
                ExpireDate = token.ValidTo

            });



        }





    }
}