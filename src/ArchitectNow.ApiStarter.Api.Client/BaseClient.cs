using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ArchitectNow.ApiStarter.Api.Client
{
    public abstract class BaseClient : IBaseClient
    {
        public string BaseUrl { get; set; }
        public string Token { get; set; }

        protected async Task<HttpClient> CreateHttpClientAsync(CancellationToken token)
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(Token))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            }

            return await Task.FromResult(client);
        }
    }
}