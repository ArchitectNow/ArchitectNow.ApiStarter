using System;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.MongoDb;
using ArchitectNow.ApiStarter.Common.Services;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ArchitectNow.ApiStarter.Common.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ILogger<User> logger,
            ICacheService cacheService,
            IOptions<MongoOptions> options,
            IValidator<User> validator = null
        ) : base(logger, cacheService, options, validator)
        {
        }

        public override string CollectionName => nameof(User);

        public override async Task ConfigureIndexes()
        {
            await base.ConfigureIndexes();

            var collection = GetCollection();
            var model = new CreateIndexModel<User>(
                Builders<User>.IndexKeys.Ascending(l => l.Email),
                new CreateIndexOptions {Name = "user_email"});
            await collection.Indexes.CreateOneAsync(model);
        }

        public async Task<User> GetByEmail(string email)
        {
            email = email.Trim().ToLower();

            return await FindOneAsync(x => x.Email == email);
        }

        public async Task<User> VerifyCredentials(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }
            
            var user = await GetByEmail(email);

            if (user == null)
                return null;

            return user.Password == password ? user : null;
        }
    }
}