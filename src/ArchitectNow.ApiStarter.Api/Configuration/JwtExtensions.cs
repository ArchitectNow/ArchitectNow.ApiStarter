using System;
using System.Linq;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.Models.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class JwtExtensions
    {
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configurationRoot,
            Func<JwtIssuerOptions, JwtSigningKey> signingKey, JwtBearerEvents jwtBearerEvents = null)
        {
            var jwtAppSettingOptions = configurationRoot.GetSection(nameof(JwtIssuerOptions)).Get<JwtIssuerOptions>();

            var issuerSigningKey = signingKey(jwtAppSettingOptions);

            services.AddSingleton(issuerSigningKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = issuerSigningKey,

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
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var task = Task.Run(() =>
                            {
                                if (context.Request.Query.TryGetValue("securityToken", out var securityToken))
                                    context.Token = securityToken.FirstOrDefault();
                            });

                            return task;
                        }
                    };
                });
        }
    }
}