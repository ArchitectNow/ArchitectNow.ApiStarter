using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;

namespace ArchitectNow.ApiStarter.Common.Services
{
    public interface ISecurityService
    {
        Task<User> Login(string email, string password);

        Task<User> Register(RegistrationVm registration);
    }
}