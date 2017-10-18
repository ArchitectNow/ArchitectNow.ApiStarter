using System;
using System.Linq;
using System.Security.Claims;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;

namespace ArchitectNow.ApiStarter.Api.Services
{
    public static class ClaimsPrincipalExtensions
    {
        public static UserInformation GetUserInformation(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return null;
            }

            var identityName = principal?.Identity?.Name;
            var role = principal?.Claims.FirstOrDefault(claim => string.Equals(nameof(UserInformation.UserRole), claim.Type, StringComparison.OrdinalIgnoreCase))?.Value;
            var idValue = principal?.Claims.FirstOrDefault(claim => string.Equals(nameof(UserInformation.Id), claim.Type, StringComparison.OrdinalIgnoreCase))?.Value;
			
            var userInformation = new UserInformation
            {
                Email = identityName,
                UserRole = role
            };

            if (Guid.TryParse(idValue, out var id))
            {
                userInformation.Id = id;
            }
			
            return userInformation;
        }
    }
}