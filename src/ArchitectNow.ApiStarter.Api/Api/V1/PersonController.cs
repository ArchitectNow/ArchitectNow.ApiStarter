using ArchitectNow.ApiStarter.Common.Repositories;
using AutoMapper;

namespace ArchitectNow.ApiStarter.Api.Api.V1
{
    public class PersonController : ApiV1BaseController
    {
        private readonly IPersonRepository _personRepository;

        public PersonController(IPersonRepository personRepository, 
            IMapper mapper, 
            IServiceInvoker serviceInvoker) : base(mapper, serviceInvoker)
        {
            _personRepository = personRepository;
        }
    }
}