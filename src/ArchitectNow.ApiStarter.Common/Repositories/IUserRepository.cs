using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.BaseDb;
using ArchitectNow.ApiStarter.Common.Models;

namespace ArchitectNow.ApiStarter.Common.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmail(string email);

        Task<User> VerifyCredentials(string email, string password);
    }
}