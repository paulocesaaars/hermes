using Deviot.Hermes.Application.Interfaces;
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
        private readonly IDeviceIntegrationService _deviceIntegrationService;

        public MainBackgroundService(ILogger<MainBackgroundService> logger, 
                                     IDeviceIntegrationService deviceIntegrationService)
        {
            _logger = logger;
            _deviceIntegrationService = deviceIntegrationService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
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
