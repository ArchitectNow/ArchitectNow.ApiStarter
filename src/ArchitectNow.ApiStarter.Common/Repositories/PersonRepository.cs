using System.Collections.Generic;
using System.Linq;
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
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        public PersonRepository(ILogger<Person> logger,
            ICacheService cacheService,
            ApiStarterContext context,
            IValidator<Person> validator = null
        ) : base(logger, cacheService, context, validator)
        {
        }

        public override string CollectionName => nameof(Person);

        public async Task<List<Person>> Search(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                var results = await FindAsync(x => x.IsActive, "Addresses");

                return results.ToList();
            }
            else
            {
                var results = await FindAsync(x => x.IsActive && x.NameFirst.Contains(searchString), "Addresses");

                return results.ToList();
            }
        }
    }
}