using System.Net.Http;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace ArchitectNow.ApiStarter.Tests
{
    public abstract class BaseIntegrationTest : BaseTest
    {
        public BaseIntegrationTest()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(BuildConfiguration());

            TestServer = new TestServer(builder);

            Client = TestServer.CreateClient();
        }

        public HttpClient Client { get; }
        public TestServer TestServer { get; }

        protected HttpContent BuildHttpContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json);
        }

        protected async Task<T> GetResponse<T>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}