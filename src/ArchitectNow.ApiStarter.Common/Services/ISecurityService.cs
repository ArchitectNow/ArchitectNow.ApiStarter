using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;

namespace ArchitectNow.ApiStarter.Common.Services
{
    public interface ISecurityService
    {
        Task<User> Login(string Email, string Password);
    }
}