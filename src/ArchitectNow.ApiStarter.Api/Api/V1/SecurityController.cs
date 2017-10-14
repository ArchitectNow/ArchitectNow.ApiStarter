using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Models;
using ArchitectNow.ApiStarter.Common.Models.Exceptions;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSwag.Annotations;

namespace ArchitectNow.ApiStarter.Api.Api.V1
{
    public class SecurityController : ApiV1BaseController
    {
        private readonly ISecurityService _securityService;
        private readonly JwtIssuerOptions _jwtOptions;
        
        public SecurityController(ISecurityService securityService, 
            IMapper mapper, 
            IServiceInvoker serviceInvoker,
            IOptions<JwtIssuerOptions> jwtOptions) : base(mapper, serviceInvoker)
        {
            _securityService = securityService;
            _jwtOptions = jwtOptions.Value;
        }
        
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(LoginResultVm))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Dictionary<string, string>))]
        public async Task<IActionResult> Login([FromBody] LoginVm parameters)
        {
            return await ServiceInvoker.AsyncOk(async () =>
            {
                var user = await _securityService.Login(parameters.Email, parameters.Password);

                if (user != null)
                {
                    var result = new LoginResultVm();
                    
                    var tokenRequest = await BuildTokenRequest(user);
                    
                    result.CurrentUser = Mapper.Map<UserVm>(user);
                    result.AuthToken = tokenRequest.Token;
                    
                    return result;
                }

                throw new ApiException<string>("Invalid Credentials");
            });
        }
        
        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(LoginResultVm))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(Dictionary<string, string>))]
        public async Task<IActionResult> Register([FromBody] RegistrationVm parameters)
        {
            return await ServiceInvoker.AsyncOk(async () => await _securityService.Register(parameters));
        }
        
        private async Task<TokenVm> BuildTokenRequest(User user)
        {
            var identity = GenerateClaimsIdentity(user);

            return await GenerateTokenRequest(user, identity);
        }

        private ClaimsIdentity GenerateClaimsIdentity(User user)
        {

            var claims = new List<Claim>
            {
                new Claim("Role", user.UserRole.ToString()),
                new Claim("Id", user.Id.ToString())
            };

            return new ClaimsIdentity(new GenericIdentity(user.Email, "Token"), claims);
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() -
                                     new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }

        private async Task<TokenVm> GenerateTokenRequest(User user, ClaimsIdentity identity)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(),
                    ClaimValueTypes.Integer64)
            };

            foreach (var claim in identity.Claims)
                claims.Add(claim);

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var token = new TokenVm
            {
                ExpiresAt = DateTimeOffset.UtcNow.AddSeconds((int)_jwtOptions.ValidFor.TotalSeconds),
                Token = encodedJwt,
                Email = user.Email 
            };

            return token;
        }
    }
}
