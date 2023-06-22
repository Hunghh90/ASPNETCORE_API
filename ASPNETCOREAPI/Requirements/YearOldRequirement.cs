using System;
using Microsoft.AspNetCore.Authorization;
namespace ASPNETCOREAPI.Requirements
{
    public class YearOldRequirement : IAuthorizationRequirement
    {
        public YearOldRequirement(int min, int max) 
        {
            MinYear = min;
            MaxYear = max;
        }
        public int MinYear { get; set; }
        public int MaxYear { get; set; }
    }
}
