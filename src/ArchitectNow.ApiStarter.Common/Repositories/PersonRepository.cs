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
        public override string CollectionName => nameof(Person);
        
        public PersonRepository(ILogger<Person> logger,
            IDataContext dataContext, ICacheService cacheService,
            IOptions<MongoOptions> options,
            IValidator<Person> validator = null
        ) : base(logger, dataContext, cacheService, options, validator)
        {
            
        }
        
        public override async Task ConfigureIndexes()
        {
            await base.ConfigureIndexes();
            
            var collection = GetCollection();
            
            await collection.Indexes.CreateOneAsync(
                Builders<Person>.IndexKeys.Ascending(l => l.NameLast),
                new CreateIndexOptions {Name = "person_nameLast"});
        }
    }
}