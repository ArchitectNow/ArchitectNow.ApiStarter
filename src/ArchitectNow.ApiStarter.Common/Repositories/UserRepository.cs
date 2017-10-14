using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.MongoDb;
using ArchitectNow.ApiStarter.Common.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArchitectNow.ApiStarter.Common.Repositories
{
    public class UserRepository : BaseRepository<User, IDataContext>, IUserRepository
    {
        public override string CollectionName => nameof(User);
        
        protected UserRepository(ILogger<User> logger,
            IDataContext dataContext, ICacheService cacheService,
            IOptions<MongoOptions> options,
            IValidator<User> validator = null
        ) : base(logger, dataContext, cacheService, options, validator)
        {
            
        }
    }
}