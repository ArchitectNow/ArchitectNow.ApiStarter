using System;
using System.Net;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Api.Services;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace ArchitectNow.ApiStarter.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    public class InvoiceController : ApiV1BaseController 
    {
        public InvoiceController(
            IMapper mapper,
            IServiceInvoker serviceInvoker) : base(mapper, serviceInvoker)
        {

        }


        /// <summary>
        /// This is the best API method EVER
        /// </summary>
        /// <returns>Some stuff</returns>
        [HttpGet()]
        [SwaggerResponse(HttpStatusCode.OK, typeof(UserVm))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ApiError))]
        public async Task<UserVm> DoWork()
        {
            return null;
        }
    }
}