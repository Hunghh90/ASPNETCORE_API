using System;
using ASPNETCOREAPI.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace ASPNETCOREAPI.Handlers
{
    public class ValidBirthdayHandle : AuthorizationHandler<YearOldRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, YearOldRequirement requirement)
        {
            if (IsValidBirthday(context.User, requirement))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
        private bool IsValidBirthday(ClaimsPrincipal user, YearOldRequirement requirement)
        {
            if (user == null) return false;
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var context = new ASPNETCOREAPI.Entities.AspnetcoreApiContext();
            var userData = context.Users.Find(Convert.ToInt32(userId));
            if (userData == null || userData.Birthday == null) return false;  
            var birthday = userData.Birthday;
            var diff = DateTime.Today.Year - birthday.Year;
            if (diff >= requirement.MinYear && diff <= requirement.MaxYear) return true;
            return false;
        }
    }
}
