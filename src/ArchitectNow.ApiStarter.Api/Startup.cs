using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArchitectNow.ApiStarter.Api.Configuration;
using ArchitectNow.ApiStarter.Api.Filters;
using ArchitectNow.ApiStarter.Common;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ArchitectNow.ApiStarter.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            _logger.LogInformation("Starting: Configure Services");

            services.AddOptions();
            
            services.ConfigureAutomapper(config => { });

            services.ConfigureApi();
            
            services.ConfigureCompression();
           
            
            var _container = services.CreateAutofacContainer(_configuration, builder => { }, new Module[]
            {
                new CommonModule(),
                new ApiModule()
            });
            
            _logger.LogInformation("Completing: Configure Services");
        }
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _logger.LogInformation("Starting: Configure");
            
            
            
            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
            
            _logger.LogInformation("Completing: Configure");

        }

       
    }
}