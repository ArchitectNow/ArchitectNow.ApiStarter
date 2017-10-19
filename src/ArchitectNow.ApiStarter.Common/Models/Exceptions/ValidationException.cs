using System.Collections.Generic;
using System.Net;
using ArchitectNow.ApiStarter.Common.Models.Validation;
using Newtonsoft.Json;

namespace ArchitectNow.ApiStarter.Common.Models.Exceptions
{
    public class ValidationException : ApiException<IEnumerable<ValidationError>>
    {
        public ValidationException(string message, IEnumerable<ValidationError> validationErrors) : base(
            HttpStatusCode.BadRequest, message, validationErrors ?? new List<ValidationError>())
        {
        }

        public override string GetContent()
        {
            return JsonConvert.SerializeObject(Content);
        }
    }
}