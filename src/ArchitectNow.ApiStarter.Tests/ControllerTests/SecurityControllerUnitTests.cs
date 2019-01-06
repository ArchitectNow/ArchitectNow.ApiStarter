using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Controllers.V2;
using ArchitectNow.ApiStarter.Api.Filters;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Api.Services;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.Exceptions;
using ArchitectNow.ApiStarter.Common.Services;
using Autofac.Extras.Moq;
using Moq;
using Xunit;

namespace ArchitectNow.ApiStarter.Tests.ControllerTests
{
    public class SecurityControllerUnitTests
    {
        [Fact]
        public async Task Login_ThrowsException_WhenUserNotFound()
        {
            using (var mockContainer = AutoMock.GetLoose())
            {
                // Create a reference to the exceptionResult builder so we can assert on it later
                var exceptionResultBuilder = mockContainer.Mock<IExceptionResultBuilder>();

                // Provide a real implementation of ServiceInvoker with it's dependencies mocked out
                var serviceInvoker = mockContainer.Create<ServiceInvoker>();
                mockContainer.Provide<IServiceInvoker>(serviceInvoker);

                // Setup security service to return null for any request to login
                var securityService = mockContainer.Mock<ISecurityService>();
                securityService.Setup(service => service.Login(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync((User) null);

                // Create the system controller with all of its dependencies mocked
                var target = mockContainer.Create<SecurityController>();

                await target.Login(new LoginVm());

                exceptionResultBuilder.Verify(builder =>
                    builder.Build(It.Is<ApiException<string>>(exception =>
                        exception.Message == "Invalid Credentials")));
            }
        }
    }
}