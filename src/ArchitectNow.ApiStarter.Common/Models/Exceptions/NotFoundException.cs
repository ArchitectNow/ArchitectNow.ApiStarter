using System.Net;

namespace ArchitectNow.ApiStarter.Common.Models.Exceptions
{
    public class NotFoundException : ApiException
    {
        public NotFoundException(string type, object key):base(HttpStatusCode.NotFound, $"Could not find {key} for {type}.")
        {
                
        }
        
        public override string GetContent()
        {
            return null;
        }
    }
}