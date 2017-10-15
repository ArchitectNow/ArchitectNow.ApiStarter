using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.MongoDb;

namespace ArchitectNow.ApiStarter.Common.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByEmail(string email);

        Task<User> VerifyCredentials(string email, string password);
    }
}