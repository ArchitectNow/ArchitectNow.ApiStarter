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

namespace ArchitectNow.ApiStarter.Api.Controllers
{
    [SwaggerResponse(HttpStatusCode.NotFound, typeof(void))]
    [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ValidationError[]))]
    [SwaggerResponse(HttpStatusCode.InternalServerError, typeof(ApiError))]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public abstract class ApiBaseController : Controller
    {
        protected ApiBaseController(IMapper mapper, IServiceInvoker serviceInvoker)
        {
            Mapper = mapper;
            ServiceInvoker = serviceInvoker;
        }

        protected IMapper Mapper { get; }
        protected IServiceInvoker ServiceInvoker { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                if (!ModelState.IsValid)
                {
                    var errorList = ModelState.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e =>
                            string.IsNullOrEmpty(e.ErrorMessage)
                                ? e.Exception?.GetBaseException().Message
                                : e.ErrorMessage).ToArray()
                    );

                    throw new ApiException<IDictionary<string, string[]>>("Invalid request", errorList);
                }

            base.OnActionExecuting(context);
        }
    }
}