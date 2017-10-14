using System;
using System.Reflection;
using ArchitectNow.ApiStarter.Api.Configuration;
using ArchitectNow.ApiStarter.Common;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Module = Autofac.Module;

namespace ArchitectNow.ApiStarter.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;
        protected IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Starting: Configure Services");
            
            services.AddOptions();
            
            services.ConfigureAutomapper(config => { });

            services.ConfigureApi();
            
            services.ConfigureCompression();
            
            ApplicationContainer = services.ConfigureAutofacContainer(_configuration, b => { }, new Module[]
            {
                new CommonModule(), new ApiModule()
            });
            
            var provider = new AutofacServiceProvider(ApplicationContainer);

            _logger.LogInformation("Completing: Configure Services");

            return provider;
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            _logger.LogInformation("Starting: Configure");
            
            app.ConfigureAssets();
            
            app.ConfigureSwagger(Assembly.GetExecutingAssembly());
            
            app.ConfigureCompression();

            app.UseMvc();
            
            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
           
            _logger.LogInformation("Completing: Configure");

        }
    }
}