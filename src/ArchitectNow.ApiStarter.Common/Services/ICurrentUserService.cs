using System.Security.Claims;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;

namespace ArchitectNow.ApiStarter.Common.Services
{
    public interface ICurrentUserService
    {
        Task<ClaimsPrincipal> GetCurrentUser();
        Task<UserInformation> GetUserInformation();
    }
}