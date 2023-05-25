using ASPNETCOREAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using ASPNETCOREAPI.Dtos;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;

namespace ASPNETCOREAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AspnetcoreApiContext _context;
        public AuthController(AspnetcoreApiContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("/register")]
        public  IActionResult Register (UserRegister user)
        {
            var hashed = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(new Entities.User { Email= user.Email, Name= user.Name, Password= hashed});
            _context.SaveChanges();
            return Ok(new UserData { Name = user.Name, Email = user.Email });
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(UserLogin userLogin)
        {
            var user = _context.Users.Where(user => user.Email == userLogin.Email).First();
            if (user == null)
            {
                return NotFound("User not exist");
            }
            bool verified = BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password);
            if(!verified)
            {
                return NotFound("User not exist");
            }
            return Ok();


        }
        [HttpPut]
        [Route("/changerPassword")]
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
    }
}
