using System.Text;
using ArchitectNow.ApiStarter.Common.Models.Options;
using ArchitectNow.ApiStarter.Common.Models.Security;
using ArchitectNow.ApiStarter.Common.MongoDb;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ArchitectNow.ApiStarter.Common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly).AsImplementedInterfaces();

            builder.RegisterType<DataContext>().As<IDataContext>()
                .InstancePerLifetimeScope();

            builder.Register(ctx => ctx.Resolve<IConfiguration>().CreateOptions<MongoOptions>("mongo"))
                .AsSelf().SingleInstance();

            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfiguration>();
                var issuerOptions = configuration.GetSection("jwtIssuerOptions").Get<JwtIssuerOptions>();
                
                var keyString = issuerOptions.Audience;
                var keyBytes = Encoding.Unicode.GetBytes(keyString);
                
                var key = new JwtSigningKey(keyBytes);

                issuerOptions.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                return new OptionsWrapper<JwtIssuerOptions>(issuerOptions);
            }).As<IOptions<JwtIssuerOptions>>().InstancePerLifetimeScope();
        }
    }
}