using Newtonsoft.Json;

namespace ArchitectNow.ApiStarter.Api.Models.ViewModels
{
    public class ApiError
    {
        /// <summary>
        ///     Error description
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        ///     Optional stack trace (only available in dev)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string StackTrace { get; set; }
    }
}