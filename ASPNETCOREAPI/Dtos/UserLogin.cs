using System;
namespace ASPNETCOREAPI.Dtos
{
    public class UserLogin
    {
 
        public string Email { get; set; }

        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}
