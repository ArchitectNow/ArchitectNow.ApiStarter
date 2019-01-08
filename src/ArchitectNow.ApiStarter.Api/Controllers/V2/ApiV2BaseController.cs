using ArchitectNow.ApiStarter.Api.Services;
using AutoMapper;

namespace ArchitectNow.ApiStarter.Api.Controllers.V2
{
    public abstract class ApiV2BaseController : ApiBaseController
    {
        protected ApiV2BaseController(IMapper mapper, IServiceInvoker serviceInvoker) : base(mapper, serviceInvoker)
        {
        }
    }
}