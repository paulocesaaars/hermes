using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Extensions
{
    public class MainBackgroundService : BackgroundService
    {
        private readonly ILogger<MainBackgroundService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IMigrationService _migrationService;
        private readonly IDeviceIntegrationService _deviceIntegrationService;

        public MainBackgroundService(ILogger<MainBackgroundService> logger, 
                                     IWebHostEnvironment environment, 
                                     IMigrationService migrationService,
                                     IDeviceIntegrationService deviceIntegrationService)
        {
            _logger = logger;
            _environment = environment;
            _migrationService = migrationService;
            _deviceIntegrationService = deviceIntegrationService;
        }

        private void ExecuteMigration()
        {
            if (_environment.EnvironmentName == "Development")
                _migrationService.Deleted();

            _migrationService.Execute();

            if (_environment.EnvironmentName == "Testing")
                _migrationService.Populate();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                ExecuteMigration();

                await _deviceIntegrationService.StartAsync();

                await Task.Delay(Timeout.Infinite, stoppingToken);

                await _deviceIntegrationService.StopAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
    }
}
