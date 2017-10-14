using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;

namespace ArchitectNow.ApiStarter.Common.Services
{
    public class SecurityService : ISecurityService
    {
        public async Task<User> Login(string Email, string Password)
        {
            return null;
        }
    }
}