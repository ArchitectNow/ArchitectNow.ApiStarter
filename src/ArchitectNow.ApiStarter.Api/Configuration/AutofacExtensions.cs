﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class AutofacExtensions
    {
        public static IContainer CreateAutofacContainer(this IServiceCollection services,
            Action<ContainerBuilder, IServiceCollection> additionalAction, params Module[] modules)
        {
            var builder = new ContainerBuilder();

            foreach (var module in modules) builder.RegisterModule(module);

            additionalAction?.Invoke(builder, services);

            builder.Populate(services);

            return builder.Build();
        }
    }
}