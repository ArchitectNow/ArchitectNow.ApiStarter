using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.Exceptions;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Repositories;
using Microsoft.Extensions.Options;

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

            if (string.IsNullOrEmpty(Registration.Email))
            {
                throw new ApiException<string>("Email not provided");
            }

            var user = new User();

            user.Email = Registration.Email;
            user.Password = Registration.Password;
            user.NameFirst = Registration.NameFirst;
            user.NameLast = Registration.NameLast;
            
            return await _userRepository.SaveAsync(user);
        }
       
    }
}