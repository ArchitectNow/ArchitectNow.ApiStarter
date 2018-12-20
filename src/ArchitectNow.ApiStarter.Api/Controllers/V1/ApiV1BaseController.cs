using System.Collections.Generic;
using System.Linq;
using System.Net;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Api.Services;
using ArchitectNow.ApiStarter.Common.Models.Exceptions;
using ArchitectNow.ApiStarter.Common.Models.Validation;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NSwag.Annotations;

namespace ArchitectNow.ApiStarter.Api.Controllers.V1
{
    public abstract class ApiV1BaseController : ApiBaseController
    {
        protected ApiV1BaseController(IMapper mapper, IServiceInvoker serviceInvoker) : base(mapper, serviceInvoker)
        {

        }
    }
}