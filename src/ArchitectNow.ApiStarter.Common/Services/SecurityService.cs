using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;
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
        
        public async Task<User> Login(string Email, string Password)
        {
            return await _userRepository.VerifyCredentials(Email, Password);
        }

        public async Task<User> Register(RegistrationVm Registration)
        {
            //TODO:  Demonstrate fluent validation

            var user = new User();

            user.Email = Registration.Email;
            user.Password = Registration.Password;
            user.NameFirst = Registration.NameFirst;
            user.NameLast = Registration.NameLast;
            
            return await _userRepository.SaveAsync(user);
        }
    }
}