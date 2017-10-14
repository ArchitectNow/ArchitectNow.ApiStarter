using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ArchitectNow.ApiStarter
{
    public interface IServiceInvoker
    {
        Task<IActionResult> AsyncOk(Func<Task> serviceCall);
        Task<IActionResult> AsyncOk<TResult>(Func<Task<TResult>> serviceCall);
        Task<IActionResult> AsyncOkCreated<TResult>(Func<Task<TResult>> serviceCall);
        Task<IActionResult> AsyncOkNoContent(Func<Task> serviceCall);
        Task<IActionResult> AsyncOkNotFound<TResult>(Func<Task<TResult>> serviceCall);
        Task<IActionResult> AsyncOkAccepted<TResult>(string location, Func<Task<TResult>> serviceCall);
        Task<IActionResult> AsyncStatusCode<TResult>(HttpStatusCode statusCode, Func<Task<TResult>> serviceCall);
        Task<IActionResult> AsyncStatusCode<TResult>(HttpStatusCode statusCode, Func<Task> serviceCall);
        Task<IActionResult> AsyncResult<TResult>(Func<Task<TResult>> serviceCall, Func<TResult, ActionResult> createResult );
        Task<IActionResult> AsyncResult(Func<Task> serviceCall, Func<ActionResult> createResult );
    }
}