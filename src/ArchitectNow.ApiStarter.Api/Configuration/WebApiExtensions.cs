using ArchitectNow.ApiStarter.Api.Filters;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class WebApiExtensions
    {
        public static void ConfigureApi(this IServiceCollection services)
        {
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMvc(o =>
                {
                    o.Filters.AddService(typeof(GlobalExceptionFilter));
                    o.ModelValidatorProviders.Clear();

                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    
                    o.Filters.Add(new AuthorizeFilter(policy));
                    
                    
                })
                .AddJsonOptions(options =>
                {
                    var settings = options.SerializerSettings;

                    var camelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();

                    settings.ContractResolver = camelCasePropertyNamesContractResolver;
                    settings.Converters = new JsonConverter[]
                    {
                        new IsoDateTimeConverter(),
                        new StringEnumConverter(true)
                    };
                })
                .AddFluentValidation();
        }

        public static void ConfigureAssets(this IApplicationBuilder app)
        {
            app.UseFileServer();

            app.UseStaticFiles();
        }
    }
}