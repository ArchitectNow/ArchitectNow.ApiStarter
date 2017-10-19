using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Services;
using Autofac;
using FluentAssertions;
using Xunit;

namespace ArchitectNow.ApiStarter.Tests.ServiceTests
{
    public class SecurityServiceTests : BaseTest
    {
        [Fact]
        public async Task Login_Should_ReturnEmail()
        {
            var securityService = Scope.Resolve<ISecurityService>();

            var loginResult = await securityService.Login("kvgros@architectnow.net", "testtest");

            loginResult.Email.Should().Be("kvgros@architectnow.net");
        }
    }
}