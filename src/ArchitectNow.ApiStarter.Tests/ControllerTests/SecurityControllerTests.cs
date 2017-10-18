using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using FluentAssertions;
using Xunit;

namespace ArchitectNow.ApiStarter.Tests.ControllerTests
{
    public class SecurityControllerTests : BaseIntegrationTest
    {
        [Fact]
        public async Task LoginApiTest_Should_ReturnUser()
        {
            var loginParams = new LoginVm
            {
                Email = "dummy",
                Password = "asdfad"
            };

            var response = await Client.PostAsync("/api/v1/security/login", BuildHttpContent(loginParams));

            response.EnsureSuccessStatusCode();

            var result = await GetResponse<LoginResultVm>(response);

            result.CurrentUser.Should().NotBeNull();
        }
    }
}