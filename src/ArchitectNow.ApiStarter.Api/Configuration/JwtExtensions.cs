using System;
using System.Linq;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class JwtExtensions
    {
          public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration,
            Func<JwtIssuerOptions, SecurityKey> signingKey)
        {
            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions)).Get<JwtIssuerOptions>();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey(jwtAppSettingOptions),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var task = Task.Run(() =>
                            {
                                if (context.Request.Query.TryGetValue("securityToken", out StringValues securityToken))
                                {
                                    context.Token = securityToken.FirstOrDefault();
                                }
                            });

                            return task;
                        }
                    };
                });
        }
    }
}