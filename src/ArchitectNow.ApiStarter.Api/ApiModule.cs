using ArchitectNow.ApiStarter.Api.Filters;
using Autofac;
using Microsoft.AspNetCore.Http;

namespace ArchitectNow.ApiStarter.Api
{
    public class ApiModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<GlobalExceptionFilter>().AsSelf().InstancePerLifetimeScope();
        }
    }
}