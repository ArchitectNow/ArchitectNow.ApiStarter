using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

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
        
        [HttpGet("securitytest")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(bool))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Dictionary<string, string>))]
        public async Task<IActionResult> SecurityTest()
        {
            return await ServiceInvoker.AsyncOk(async () => true);
        }
    }
}