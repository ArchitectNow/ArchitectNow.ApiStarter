using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.CrossPlatEngine;
using Newtonsoft.Json;

namespace ArchitectNow.ApiStarter.Tests
{
    public abstract class BaseIntegrationTest : BaseTest
    {
        
        public HttpClient Client { get; private set; }
        public TestServer TestServer { get; private set; }
        
        public BaseIntegrationTest()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(BuildConfiguration());
     
            TestServer = new TestServer(builder);

            Client = TestServer.CreateClient();
        }

        protected HttpContent BuildHttpContent(object Data)
        {
            var json = JsonConvert.SerializeObject(Data);

            return new StringContent(json);
        }

        protected async Task<T> GetResponse<T>(HttpResponseMessage response)
        {
            var result =  await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}