using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class AutofacExtensions
    {
        public static IContainer ConfigureAutofacContainer(this IServiceCollection services, IConfiguration configuration, Action<ContainerBuilder> additionalAction, params Module[] modules)
        {
            var builder = new ContainerBuilder();
            
            builder.Register(ctx => configuration).As<IConfiguration>();

            foreach (var module in modules)
            {
                builder.RegisterModule(module);
            }

            additionalAction?.Invoke(builder);

            builder.Populate(services);

            return builder.Build();
        }
    }
}