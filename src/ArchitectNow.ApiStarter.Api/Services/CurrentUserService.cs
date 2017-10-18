using System.Security.Claims;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Services;
using Microsoft.AspNetCore.Http;

namespace ArchitectNow.ApiStarter.Api.Services
{
    class CurrentUserService: ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Task<ClaimsPrincipal> GetCurrentUser()
        {
            var httpContextUser = _contextAccessor.HttpContext.User;
            return httpContextUser.AsResult();
        }

        public Task<UserInformation> GetUserInformation()
        {
            var principal = _contextAccessor.HttpContext.User;
            var userInformation = principal.GetUserInformation();
            return userInformation.AsResult();
        }
    }
}