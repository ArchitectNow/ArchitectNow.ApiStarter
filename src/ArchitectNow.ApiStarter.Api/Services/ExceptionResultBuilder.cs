using System;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Models.Exceptions;
using Autofac.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ArchitectNow.ApiStarter.Api.Services
{
    public class ExceptionResultBuilder : IExceptionResultBuilder
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<ExceptionResultBuilder> _logger;

        public ExceptionResultBuilder(IHostingEnvironment hostingEnvironment, ILogger<ExceptionResultBuilder> logger)
        {
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public IActionResult Build(Exception exception)
        {
            var stackTrace = "No stack trace available";

            if (!string.Equals(_hostingEnvironment.EnvironmentName, "Production", StringComparison.OrdinalIgnoreCase))
                stackTrace = exception.GetBaseException().StackTrace;
            var statusCode = 500;
            string content = null;
            var message = exception.GetBaseException().Message;

            var dependencyResolutionException = exception as DependencyResolutionException;
            if (dependencyResolutionException != null)
                message = $"Dependency Exception: Please ensure that classes implement the interface: {message}";

            var notFoundException = exception as NotFoundException;
            if (notFoundException != null)
                return new NotFoundResult();

            var apiException = exception as ApiException;

            if (apiException != null)
            {
                statusCode = (int) apiException.StatusCode;
                content = apiException.GetContent();
                if (!string.IsNullOrEmpty(apiException.Message))
                    message = apiException.GetBaseException().Message;
            }

            return CreateActionResult(content, message, stackTrace, statusCode, exception);
        }

        protected virtual IActionResult CreateActionResult(string content, string message, string stackTrace,
            int statusCode, Exception exception)
        {
            var apiError = new ApiError
            {
                Error = content ?? message
            };

            if (!string.IsNullOrEmpty(stackTrace))
                apiError.StackTrace = stackTrace;

            var objectResult = new ObjectResult(apiError)
            {
                StatusCode = statusCode
            };
            var eventId = new EventId(statusCode);

            _logger.LogError(eventId, exception, message);

            return objectResult;
        }
    }
}