using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Interfaces;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Services
{
    public class MainBackgroundService : BackgroundService
    {
        private List<IDrive> _drives = new List<IDrive>();
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MainBackgroundService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IMigrationService _migrationService;

        public MainBackgroundService(IServiceProvider serviceProvider, ILogger<MainBackgroundService> logger, IWebHostEnvironment environment, IMigrationService migrationService)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _environment = environment;
            _migrationService = migrationService;
        }

        private void ExecuteMigration()
        {
            if (_environment.EnvironmentName == "Development")
                _migrationService.Deleted();

            _migrationService.Execute();

            if (_environment.EnvironmentName == "Testing")
                _migrationService.Populate();
        }

        private async Task InitializeDrivesAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IRepositorySQLite>();
                    var driveFactory = scope.ServiceProvider.GetRequiredService<IDriveFactory>();
                    
                    var devices = await repository.Get<Device>().ToListAsync();

                    foreach (var device in devices)
                        _drives.Add(driveFactory.GenerateDrive(device));
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                ExecuteMigration();

                await InitializeDrivesAsync();

                if (_drives.Any())
                {
                    var startTasks = new List<Task>();
                    _drives.ForEach(x => startTasks.Add(x.StartAsync()));

                    var startTask = Task.WhenAll(startTasks);
                    startTask.Wait();
                }

                await Task.Delay(Timeout.Infinite, stoppingToken);

                if (_drives.Any())
                {
                    var stopTasks = new List<Task>();
                    _drives.ForEach(x => stopTasks.Add(x.StopAsync()));

                    var stopTask = Task.WhenAll(stopTasks);
                    stopTask.Wait();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }

        public async Task AddDriveAsync(Device device)
        {
            if(!_drives.Any(x => x.Device.Id == device.Id))
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var driveFactory = scope.ServiceProvider.GetRequiredService<IDriveFactory>();
                        var drive = driveFactory.GenerateDrive(device);
                        _drives.Add(drive);
                        await drive.StartAsync();
                    }
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception.Message);
                }
            }
        }

        public async Task UpdateDriveAsync(Device device)
        {
            try
            {
                var currentDrive = _drives.FirstOrDefault(x => x.Device.Id == device.Id);
                if (currentDrive is not null)
                {
                    await currentDrive.StopAsync();
                    _drives.Remove(currentDrive);
                    await AddDriveAsync(device);
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public async Task DeleteDriveAsync(Guid id)
        {
            try
            {
                var currentDrive = _drives.FirstOrDefault(x => x.Device.Id == id);
                if (currentDrive is not null)
                {
                    await currentDrive.StopAsync();
                    _drives.Remove(currentDrive);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }
    }
}
