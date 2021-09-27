using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Deviot.Hermes.Api.Configurations
{
    [ExcludeFromCodeCoverage]

    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Por favor insira o token JWT dessa maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                     },
                      new string[] { }
                   }
                });

                var info = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Projeto Hermes Modbus TCP",
                    Description = "API referente ao projeto Hermes Modbus TCP",
                    Version = info.ProductVersion
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API referente ao projeto Hermes Modbus TCP");
                //options.InjectStylesheet("custom.css");
            });

            app.UseStaticFiles();

            return app;
        }
    }
}
