using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Interfaces;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Services
{
    public class DeviceIntegrationService : IDeviceIntegrationService
    {
        private List<IDrive> _drives = new List<IDrive>();

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DeviceIntegrationService> _logger;

        public DeviceIntegrationService(ILogger<DeviceIntegrationService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
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

        public async Task StartAsync()
        {
            await InitializeDrivesAsync();
            if (_drives.Any())
                foreach (var driver in _drives)
                    driver.Start();
        }

        public async Task StopAsync()
        {
            if (_drives.Any())
                foreach (var driver in _drives)
                    driver.Stop();
        }

        public async Task AddDriveAsync(Device device)
        {
            if (!_drives.Any(x => x.Id == device.Id))
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var driveFactory = scope.ServiceProvider.GetRequiredService<IDriveFactory>();
                        var drive = driveFactory.GenerateDrive(device);
                        _drives.Add(drive);
                        drive.Start();
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
                var currentDrive = _drives.FirstOrDefault(x => x.Id == device.Id);
                if (currentDrive is not null)
                    await currentDrive.UpdateDriveAsync(device);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }

        public async Task DeleteDriveAsync(Guid id)
        {
            try
            {
                var currentDrive = _drives.FirstOrDefault(x => x.Id == id);
                if (currentDrive is not null)
                {
                    currentDrive.Stop();
                    _drives.Remove(currentDrive);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }
        }

        public async Task<object> GetDataAsync(Guid id)
        {
            try
            {
                var currentDrive = _drives.FirstOrDefault(x => x.Id == id);
                if (currentDrive is not null)
                    return await currentDrive.GetDataAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }

            return null;
        }
    }
}
