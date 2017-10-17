using System.Net;

namespace ArchitectNow.ApiStarter.Common.Models.Exceptions
{
    public interface IApiException<TContent>
    {
        HttpStatusCode StatusCode { get; set; }
        TContent Content { get; set; }
    }
}