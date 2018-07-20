using System.Collections.Generic;
using System.Linq;
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
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(ILogger<Person> logger,
            IDataContext dataContext, ICacheService cacheService,
            IOptions<MongoOptions> options,
            IValidator<Person> validator = null
        ) : base(logger, dataContext, cacheService, options, validator)
        {
        }

        public override string CollectionName => nameof(Person);

        public override async Task ConfigureIndexes()
        {
            await base.ConfigureIndexes();

            var collection = GetCollection();

            await collection.Indexes.CreateOneAsync(
                new CreateIndexModel<Person>(
                Builders<Person>.IndexKeys.Ascending(l => l.NameLast),
                new CreateIndexOptions {Name = "person_nameLast"}));
        }

        public async Task<List<Person>> Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                var results = await FindAsync(x => x.IsActive);

                return results.ToList();
            }
            else
            {
                var results = await FindAsync(x => x.IsActive && x.NameFirst.Contains(searchString));

                return results.ToList();
            }
        }
    }
}