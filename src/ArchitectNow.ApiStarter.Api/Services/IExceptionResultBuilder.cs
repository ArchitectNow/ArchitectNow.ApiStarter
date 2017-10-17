using System;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectNow.ApiStarter.Api.Services
{
    public interface IExceptionResultBuilder
    {
        IActionResult Build(Exception exception);
    }
}