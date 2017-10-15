using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using Xunit;

namespace ArchitectNow.ApiStarter.Tests.ControllerTests
{
    public class SecurityControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task LoginApiTest()
        {
            var loginParams = new LoginVm();

            loginParams.Email = "dummy";
            loginParams.Password = "asdfad";

            var response = await Client.PostAsync("/api/v1/security/login", BuildHttpContent(loginParams));

            response.EnsureSuccessStatusCode();

            var result = await GetResponse<LoginResultVm>(response);

            Assert.True(result.CurrentUser != null);
        }
    }
}