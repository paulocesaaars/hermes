using Deviot.Hermes.Infra.SQLite.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Deviot.Hermes.Infra.SQLite.Configuration
{
    public static class DependencyInjectionSQLite
    {
        private static string CONNECTION_STRING = "SQLiteConnection";

        private static string CONNECTION_STRING_ERROR = "A conexão do SQLite não foi informada";

        public static IServiceCollection AddDependencyInjectionSQLite(this IServiceCollection services, IConfiguration configuration)
        {
            var sqliteConnection = configuration.GetConnectionString(CONNECTION_STRING);
            if (string.IsNullOrEmpty(sqliteConnection))
                throw new ArgumentNullException(CONNECTION_STRING_ERROR);

            services.AddDbContext<ApplicationDbContext>(opt =>
                opt.UseSqlite(sqliteConnection));

            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IRepositorySQLite, RepositorySQLite>();
            services.AddScoped<IMigrationService, MigrationService>();

            return services;
        }
    }
}
