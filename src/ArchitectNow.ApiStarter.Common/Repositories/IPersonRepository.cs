using System.Collections.Generic;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.BaseDb;
using ArchitectNow.ApiStarter.Common.Models;

namespace ArchitectNow.ApiStarter.Common.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<List<Person>> Search(string searchString);
    }
}