using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;

namespace ArchitectNow.ApiStarter.Common.Services
{
    public interface ISecurityService
    {
        Task<User> Login(string Email, string Password);

        Task<User> Register(RegistrationVm Registration);
    }
}