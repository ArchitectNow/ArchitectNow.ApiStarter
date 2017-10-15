using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Services;
using Autofac;
using Xunit;

namespace ArchitectNow.ApiStarter.Tests.ServiceTests
{
    public class SecurityServiceTests : BaseTest
    {
        [Fact]
        public async Task LoginTests()
        {
            var container = BuildContainer();

            var securityService = container.Resolve<ISecurityService>();

            var loginResult = await securityService.Login("kvgros@architectnow.net", "testtest");

            Assert.True(loginResult.Email.ToLower() == "kvgros@architectnow.net");
        }
    }
}