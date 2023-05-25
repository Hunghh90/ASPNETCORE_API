using System;
namespace ASPNETCOREAPI.Dtos
{
    public class UserChangerPassword
    {
        public string Email { get; set; }

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }

    }
}
