using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Services;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Repositories;
using ArchitectNow.ApiStarter.Common.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace ArchitectNow.ApiStarter.Api.Controllers.V1
{
    public class PersonController : ApiV1BaseController
    {
        private readonly ICurrentUserService _currentUserService;

        public PersonController(ICurrentUserService currentUserService,
            IMapper mapper,
            IServiceInvoker serviceInvoker) : base(mapper, serviceInvoker)
        {
            _currentUserService = currentUserService;
        }

        [HttpGet("securitytest")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(UserInformation))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Dictionary<string, string>))]
        public async Task<IActionResult> SecurityTest()
        {
            return await ServiceInvoker.AsyncOk(() => _currentUserService.GetUserInformation());
        }
    }
}