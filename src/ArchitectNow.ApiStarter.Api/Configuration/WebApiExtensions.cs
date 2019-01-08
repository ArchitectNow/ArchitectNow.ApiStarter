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
        public static void ConfigureApi(this IServiceCollection services,
            FluentValidationOptions fluentValidationOptions, Action<MvcOptions> configureMvc = null,
            Action<MvcJsonOptions> configureJson = null)
        {
            /*************************
             * IConfiguration is not available yet
             *************************/

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.UseApiBehavior = false;
            });

            var mvcBuilder = services.AddMvcCore(o =>
                {
                    o.Filters.AddService(typeof(GlobalExceptionFilter));
                    o.ModelValidatorProviders.Clear();

                    configureMvc?.Invoke(o);
                })
                .AddAuthorization()
                .AddJsonFormatters()
                .AddJsonOptions(options =>
                {
                    var settings = options.SerializerSettings;

                    var camelCasePropertyNamesContractResolver = new CamelCasePropertyNamesContractResolver();

                    settings.ContractResolver = camelCasePropertyNamesContractResolver;
                    settings.Converters.Add(new IsoDateTimeConverter());
                    settings.Converters.Add(new StringEnumConverter(new DefaultNamingStrategy()));

                    configureJson?.Invoke(options);
                });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            if (fluentValidationOptions.Enabled)
                mvcBuilder.AddFluentValidation(
                    configuration => fluentValidationOptions.Configure?.Invoke(configuration));
        }
    }
}