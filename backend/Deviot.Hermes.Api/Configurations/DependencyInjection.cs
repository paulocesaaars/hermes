using Deviot.Hermes.Application.Configurations;
using Deviot.Hermes.Infra.SQLite.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Deviot.Hermes.Api.Configurations
{
    [ExcludeFromCodeCoverage]

    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDependencyInjectionApplication();
            services.AddDependencyInjectionSQLite(configuration);

            return services;
        }
    }
}
