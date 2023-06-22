using ASPNETCOREAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using ASPNETCOREAPI.Dtos;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Newtonsoft.Json.Linq;
using System.Globalization;


namespace ASPNETCOREAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AspnetcoreApiContext _context;
        private readonly IConfiguration _config;
        public AuthController(AspnetcoreApiContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [Route("register")]
        public  IActionResult Register (UserRegister user)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var u = new Entities.User { Email = user.Email, Name = user.Name, Password = hashed };
            _context.Users.Add(u);
            _context.SaveChanges();
            return Ok(new UserData { Name = user.Name, Email = user.Email, Token = GenerateJWT(u) });
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(UserLogin userLogin)
        {
            var user = _context.Users.Where(user => user.Email.Equals(userLogin.Email)).First();
            if (user == null)
            {
                return NotFound("User not exist");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password);
            if(!verified)
            {
                return NotFound("User not exist");
            }
            var token = GenerateJWT(user);
            return Ok(new UserLogin { Token= token });


        }
        [HttpPut]
        [Route("changerPassword")]
        public  IActionResult ChangerPassword (UserChangerPassword userChangerPassword)
        {
            var user = _context.Users.Where(user => user.Email == userChangerPassword.Email).First();
            if (user == null)
            {
                return NotFound("User not exist");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(userChangerPassword.OldPassword, user.Password);
            if (!verified)
            {
                return NotFound("User not exist");
            }

            var hashed = BCrypt.Net.BCrypt.HashPassword(userChangerPassword.NewPassword);
            user.Password = hashed;
            _context.SaveChanges();
            return Ok();
        }
        private String GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signatureKey = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "Admin"),
            };
            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: signatureKey
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpGet]
        [Route("profile")]
        public  IActionResult  Profile()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                var userClaims = identity.Claims;
                var user = new UserData
                {
                    Id = Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value),
                    Name = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value,
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                };
                return Ok(user);
            }
            return Unauthorized();
        }
    }
}
