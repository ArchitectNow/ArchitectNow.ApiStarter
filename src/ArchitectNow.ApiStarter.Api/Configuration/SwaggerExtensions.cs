using System.Reflection;
using Microsoft.AspNetCore.Builder;
using NJsonSchema;
using NSwag;
using NSwag.AspNetCore;
using NSwag.SwaggerGeneration.Processors.Security;

namespace ArchitectNow.ApiStarter.Api.Configuration
{
    public static class SwaggerExtensions
    {
        public static void ConfigureSwagger(this IApplicationBuilder app, Assembly assembly)
        {
            app.UseSwaggerUi3(assembly, settings =>
            {
                settings.GeneratorSettings.DefaultPropertyNameHandling = PropertyNameHandling.CamelCase;
                settings.GeneratorSettings.Title = "ArchitectNow.ApiStarter";
                settings.GeneratorSettings.FlattenInheritanceHierarchy = true;
                settings.GeneratorSettings.IsAspNetCore = true;
                settings.GeneratorSettings.DocumentProcessors.Add(
                    new SecurityDefinitionAppender("Authorization",
                        new SwaggerSecurityScheme
                        {
                            Type = SwaggerSecuritySchemeType.ApiKey,
                            Name = "Authorization",
                            In = SwaggerSecurityApiKeyLocation.Header
                        })
                );

                settings.SwaggerRoute = "/api/docs/v1/swagger.json";
                settings.SwaggerUiRoute = "/api/docs";
            });
        }
    }
}