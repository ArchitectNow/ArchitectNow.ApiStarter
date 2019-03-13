using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.BaseDb;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArchitectNow.ApiStarter.Common.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ILogger<User> logger,
            ICacheService cacheService,
            ApiStarterContext context,
            IValidator<User> validator = null
        ) : base(logger, cacheService, context, validator)
        {
        }

        public override string CollectionName => nameof(User);

        public async Task<User> GetByEmail(string email)
        {
            email = email.Trim().ToLower();

            return await FindOneAsync(x => x.Email == email);
        }

        public async Task<User> VerifyCredentials(string email, string password)
        {
            var user = await GetByEmail(email);

            if (user == null)
                return null;

            return user.Password == password ? user : null;
        }
    }
}