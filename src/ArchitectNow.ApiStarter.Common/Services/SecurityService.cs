using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.Exceptions;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Repositories;

namespace ArchitectNow.ApiStarter.Common.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUserRepository _userRepository;

        public SecurityService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Login(string email, string password)
        {
            return await _userRepository.VerifyCredentials(email, password);
        }

        public async Task<User> Register(RegistrationVm registration)
        {
            //TODO:  Demonstrate fluent validation

            if (string.IsNullOrEmpty(registration.Email))
                throw new ApiException<string>("Email not provided");

            var user = new User();

            user.Email = registration.Email;
            user.Password = registration.Password;
            user.NameFirst = registration.NameFirst;
            user.NameLast = registration.NameLast;

            return await _userRepository.SaveAsync(user);
        }
    }
}