using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Configuration;
using ArchitectNow.ApiStarter.Common;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.Models.Security;
using ArchitectNow.ApiStarter.Common.MongoDb;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ArchitectNow.ApiStarter.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _logger = logger;
            
            logger.LogInformation($"Constructing for environment: {hostingEnvironment.EnvironmentName}");
        }

        protected IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Starting: Configure Services");

            services.ConfigureLogging();

            services.AddOptions();

            services.ConfigureJwt(_configuration, ConfigureSecurityKey);

            services.ConfigureAutomapper(config => { });

            services.ConfigureApi();

            services.ConfigureCompression();

            services.AddMemoryCache();

            ApplicationContainer =
                services.ConfigureAutofacContainer(_configuration, b => { }, new CommonModule(), new ApiModule());

            var provider = new AutofacServiceProvider(ApplicationContainer);

            ConfigureMongoIndexes();

            _logger.LogInformation("Completing: Configure Services");

            return provider;
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IConfiguration configuration,
            ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            _logger.LogInformation("Starting: Configure");

            env.ConfigureLogger(loggerFactory, configuration);
            
            app.ConfigureSwagger(Assembly.GetExecutingAssembly());

            app.ConfigureCompression();
            
            app.ConfigureAssets();

            app.UseMvc();

            _logger.LogInformation("Completing: Configure");
        }

        protected virtual SecurityKey ConfigureSecurityKey(JwtIssuerOptions issuerOptions)
        {
            //this would be more secure, value pulled from KeyVault
            var keyString = issuerOptions.Audience;
            var keyBytes = Encoding.Unicode.GetBytes(keyString);
            var signingKey = new JwtSigningKey(keyBytes);
            return signingKey;
        }

        private void ConfigureMongoIndexes()
        {
            _logger.LogInformation("Starting: Creating Mongo Indexes");

            try
            {
                var baseRepositories = ApplicationContainer.Resolve<IEnumerable<IBaseRepository>>();
                foreach (var baseRepository in baseRepositories)
                    Task.Run(async () => await baseRepository.ConfigureIndexes());
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Could not create index");
            }
            finally
            {
                _logger.LogInformation("Completing: Creating Mongo Indexes");
            }
        }
    }
}