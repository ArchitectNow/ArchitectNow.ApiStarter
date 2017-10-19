using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.MongoDb;

namespace ArchitectNow.ApiStarter.Common.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<List<Person>> Search(string searchString);
    }
}