using System;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectNow.ApiStarter.Api.Filters
{
    public interface IExceptionResultBuilder
    {
        IActionResult Build(Exception exception);
    }
}