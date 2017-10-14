using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;

namespace ArchitectNow.ApiStarter.Api.Api.V1
{
    public class SecurityController : ApiV1BaseController
    {
        private readonly ISecurityService _securityService;

        public SecurityController(ISecurityService securityService, 
            IMapper mapper, 
            IServiceInvoker serviceInvoker) : base(mapper, serviceInvoker)
        {
            _securityService = securityService;
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(LoginResultVm))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Dictionary<string, string>))]
        public async Task<IActionResult> Login([FromBody] LoginVm parameters)
        {
            var result =  await ServiceInvoker.AsyncOk(() =>  _securityService.Login(parameters.Email,parameters.Password));

            return result;
        }
    }
}
