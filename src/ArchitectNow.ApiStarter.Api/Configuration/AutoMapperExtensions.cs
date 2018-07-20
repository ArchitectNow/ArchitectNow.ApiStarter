using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class AutoMapperExtensions
    {
        public static void ConfigureAutomapper(this IServiceCollection services,
            Action<IMapperConfigurationExpression> action)
        {
            services.AddAutoMapper(action);
        }
    }
}