using ArchitectNow.ApiStarter.Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectNow.ApiStarter.Api.Controllers.V1
{
    public abstract class ApiV1BaseController : ApiBaseController
    {
        protected ApiV1BaseController(IMapper mapper, IServiceInvoker serviceInvoker) : base(mapper, serviceInvoker)
        {
        }
    }
}