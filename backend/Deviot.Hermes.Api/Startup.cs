using Deviot.Hermes.Api.Configurations;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Deviot.Hermes.Api
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Dependency injection config
            services.AddDependencyInjection(Configuration);

            // Api - Configuration
            services.AddApiConfiguration(Configuration);

            // Versionamento
            services.AddVersioningConfiguration();

            // Swagger config
            services.AddSwaggerConfiguration();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseSwaggerConfiguration();

            app.UseApiConfiguration(environment);
        }
    }
}
