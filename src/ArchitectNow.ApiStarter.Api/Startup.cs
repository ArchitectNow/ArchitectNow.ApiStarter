using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using ArchitectNow.ApiStarter.Api.Configuration;
using ArchitectNow.ApiStarter.Api.Models.Validation;
using ArchitectNow.ApiStarter.Api.Services;
using ArchitectNow.ApiStarter.Common;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.Models.Security;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacSerilogIntegration;
using AutoMapper;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Serilog;
using Serilog.Context;

namespace ArchitectNow.ApiStarter.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<Startup> _logger;
        private IContainer _applicationContainer;

        public Startup(ILogger<Startup> logger, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation($"{nameof(ConfigureServices)} starting...");

            services.AddOptions();

            services.ConfigureJwt(_configuration, ConfigureSecurityKey);

            if (!_hostingEnvironment.IsDevelopment())
            {
                services.AddHealthChecks()
                    .AddMongoDb(_configuration["mongo:connectionString"], _configuration["mongo:databaseName"],
                        "MongoDb")
                    .AddCheck("Custom", () => { return HealthCheckResult.Healthy(); });

                services.AddHealthChecksUI();
            }

            services.ConfigureApi(new FluentValidationOptions {Enabled = true});

            AddSwaggerDocumentForVersion(services, "1.0", "1");
            AddSwaggerDocumentForVersion(services, "2.0", "2");

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Default", builder => builder.RequireAuthenticatedUser().Build());
            });

            if (!_hostingEnvironment.IsDevelopment())
            {
                var key = _configuration["ApplicationInsights:InstrumentationKey"];

                if (!string.IsNullOrEmpty(key)) services.AddApplicationInsightsTelemetry(key);
            }

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
            services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });

            services.AddCors();

            //last
            _applicationContainer = services.CreateAutofacContainer((builder, serviceCollection) =>
                {
                    builder.RegisterLogger();

                    serviceCollection.AddAutoMapper(expression =>
                    {
                        expression.ConstructServicesUsing(type => _applicationContainer.Resolve(type));
                    });
                },
                new WebModule(),
                new CommonModule());

            // Create the IServiceProvider based on the container.
            var provider = new AutofacServiceProvider(_applicationContainer);

            _logger.LogInformation($"{nameof(ConfigureServices)} complete...");

            return provider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder builder,
            IApplicationLifetime appLifetime,
            IConfiguration configuration)
        {
            var logger = builder.ApplicationServices.GetService<ILogger<Startup>>();

            logger.LogInformation($"{nameof(Configure)} starting...");

            builder.UseFileServer();

            var uploadsPath = configuration["uploadsPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

            builder.UseStaticFiles();

            builder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                RequireHeaderSymmetry = false
            });
            
            builder.UseCors(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            builder.UseAuthentication();

            builder.UseResponseCompression();

            if (!_hostingEnvironment.IsDevelopment())
            {
                builder.UseHealthChecksUI();
    
                builder.UseHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            }

            builder.UseSwagger(settings =>
            {
                settings.Path = "/docs/{documentName}/swagger.json";

                settings.PostProcess = (document, request) =>
                {
                    _logger.LogInformation("Headers: {0}", ExtractHeaders(request));
                    document.Host = ExtractHost(request);
                    document.BasePath = ExtractPath(request);
                };

            });

            builder.UseSwaggerUi3(settings =>
            {
                settings.Path = "/docs";
                settings.EnableTryItOut = true;
                settings.DocumentPath = "/docs/{documentName}/swagger.json";
                settings.TransformToExternalPath = (route, request) => ExtractPath(request) + route;
                settings.DocExpansion = "Full";
                
//                settings.GeneratorSettings.DocumentProcessors.Add(new SecurityDefinitionAppender("Authorization", new SwaggerSecurityScheme
//                {
//                    Type = SwaggerSecuritySchemeType.ApiKey,
//                    Name = "Authorization",
//                    In = SwaggerSecurityApiKeyLocation.Header
//                }));

            });

            builder.Use(async (context, next) =>
            {
                LogContext.PushProperty("Environment", _hostingEnvironment.EnvironmentName);
                if (context.User.Identity.IsAuthenticated)
                {
                    var userInformation = context.User.GetUserInformation();
                    using (LogContext.PushProperty("User", userInformation))
                    {
                        await next.Invoke();
                    }
                }
                else
                {
                    using (LogContext.PushProperty("User", "anonymous"))
                    {
                        await next.Invoke();
                    }
                }
            });

            builder.UseMvc();
           
            var option = new RewriteOptions();
            option.AddRedirect("^$", "docs");
            builder.UseRewriter(option);

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            try
            {
                _applicationContainer.Resolve<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
            }
            catch (AutoMapperConfigurationException exception)
            {
                if (_hostingEnvironment.IsDevelopment())
                {
                    logger.LogError(exception.Message);
                    throw;
                }

                logger.LogWarning(exception.Message);
            }

            logger.LogInformation($"{nameof(Configure)} complete...");
        }

        private JwtSigningKey ConfigureSecurityKey(JwtIssuerOptions issuerOptions)
        {
            var keyString = issuerOptions.Audience;
            var keyBytes = Encoding.Unicode.GetBytes(keyString);
            var signingKey = new JwtSigningKey(keyBytes);
            return signingKey;
        }

        private void AddSwaggerDocumentForVersion(IServiceCollection services, string documentName, string groupName)
        {
            services.AddSwaggerDocument(settings =>
            {
                settings.Title = "ArchitectNow API Workshop";
                settings.Description = "ASPNETCore API built as a demonstration during workshop";

                settings.SerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = {new StringEnumConverter()}
                };
                
                settings.Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
                settings.DocumentName = documentName;
                settings.ApiGroupNames = new[] {groupName};
            });
        }
        
        private string ExtractHost(HttpRequest request)
        {
            return request.Headers.ContainsKey("X-Forwarded-Host")
                ? new Uri($"{ExtractProto(request)}://{request.Headers["X-Forwarded-Host"].First()}").Host
                : request.Host.Host;
        }

        private string ExtractProto(HttpRequest request) =>
            request.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? request.Protocol;

        private string ExtractPath(HttpRequest request) =>
            request.Headers.ContainsKey("X-Forwarded-Host") ?
                new Uri($"{ExtractProto(request)}://{request.Headers["X-Forwarded-Host"].First()}").AbsolutePath :
                string.Empty;

        private string ExtractHeaders(HttpRequest request)
        {
            return string.Join("|", request.Headers.Select(x => $"{x.Key}: {x.Value.ToString()}"));
        }
    }
}