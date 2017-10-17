using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectNow.ApiStarter.Api.Services
{
    public class ServiceInvoker : IServiceInvoker
    {
        private readonly IExceptionResultBuilder _exceptionResultBuilder;

        public ServiceInvoker(IExceptionResultBuilder exceptionResultBuilder)
        {
            _exceptionResultBuilder = exceptionResultBuilder;
        }

        public virtual async Task<IActionResult> AsyncOk<TResult>(Func<Task<TResult>> serviceCall)
        {
            try
            {
                var result = await serviceCall();

                return new OkObjectResult(result);
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncOkCreated<TResult>(Func<Task<TResult>> serviceCall)
        {
            try
            {
                var result = await serviceCall();

                return new ObjectResult(result)
                {
                    StatusCode = 201
                };
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncOk(Func<Task> serviceCall)
        {
            try
            {
                await serviceCall();

                return new OkResult();
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncOkNoContent(Func<Task> serviceCall)
        {
            try
            {
                await serviceCall();

                return new NoContentResult();
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncOkNotFound<TResult>(Func<Task<TResult>> serviceCall)
        {
            try
            {
                var result = await serviceCall();

                if (EqualityComparer<TResult>.Default.Equals(result, default(TResult)))
                    return new NotFoundResult();

                return new OkObjectResult(result);
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncOkAccepted<TResult>(string location,
            Func<Task<TResult>> serviceCall)
        {
            try
            {
                var result = await serviceCall();

                return new AcceptedResult(location, result);
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncStatusCode<TResult>(HttpStatusCode statusCode,
            Func<Task<TResult>> serviceCall)
        {
            try
            {
                var result = await serviceCall();

                return new ObjectResult(result)
                {
                    StatusCode = (int) statusCode
                };
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncStatusCode<TResult>(HttpStatusCode statusCode,
            Func<Task> serviceCall)
        {
            try
            {
                await serviceCall();

                return new StatusCodeResult((int) statusCode);
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncResult<TResult>(Func<Task<TResult>> serviceCall,
            Func<TResult, ActionResult> createResult)
        {
            try
            {
                var result = await serviceCall();

                return createResult(result);
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }

        public virtual async Task<IActionResult> AsyncResult(Func<Task> serviceCall, Func<ActionResult> createResult)
        {
            try
            {
                await serviceCall();

                return createResult();
            }
            catch (Exception exception)
            {
                return _exceptionResultBuilder.Build(exception);
            }
        }
    }
}