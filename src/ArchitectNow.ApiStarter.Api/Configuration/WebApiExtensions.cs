using System;
using ArchitectNow.ApiStarter.Api.Filters;
using ArchitectNow.ApiStarter.Api.Models.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class WebApiExtensions
    {
        public static void ConfigureApi(this IServiceCollection services, FluentValidationOptions fluentValidationOptions, Action<MvcOptions> configureMvc = null, Action<MvcJsonOptions> configureJson = null)
        {
            /*************************
             * IConfiguration is not available yet
             *************************/

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
            services.AddRouting(options => options.LowercaseUrls = true);
            var mvcBuilder = services.AddMvc(o =>
                {
                    o.Filters.AddService(typeof(GlobalExceptionFilter));
                    o.ModelValidatorProviders.Clear();
                    configureMvc?.Invoke(o);
                })
                .AddJsonOptions(options =>
                {
                    var settings = options.SerializerSettings;

                    var camelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();

                    settings.ContractResolver = camelCasePropertyNamesContractResolver;
                    settings.Converters.Add(new IsoDateTimeConverter());
                    settings.Converters.Add(new StringEnumConverter(true));
					
                    configureJson?.Invoke(options);
                });


            if (fluentValidationOptions.Enabled)
            {
                mvcBuilder.AddFluentValidation(configuration => fluentValidationOptions.Configure?.Invoke(configuration));
            }
        }
    }
}