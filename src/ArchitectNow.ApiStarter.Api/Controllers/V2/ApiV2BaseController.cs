using ArchitectNow.ApiStarter.Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectNow.ApiStarter.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    public abstract class ApiV2BaseController : ApiBaseController
    {
        protected ApiV2BaseController(IMapper mapper, IServiceInvoker serviceInvoker) : base(mapper, serviceInvoker)
        {
        }
    }
}