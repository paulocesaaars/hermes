using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Domain.Interfaces;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Services
{
    public class DriverService : IDriverService
    {
        private List<IDrive> _drives = new List<IDrive>();
        private readonly IWebHostEnvironment _environment;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DriverService> _logger;

        private const string NAME = "{name}";
        private const string ERROR_INITIALIZE = "";
        private const string ERROR_START = "Houve um erro ao iniciar o driver";
        private const string ERROR_STOP = "Houve um erro ao parar o driver";
        private const string ERROR_DELETE = "Houve um erro ao deletar o dispositivo {name}.";

        public DriverService(ILogger<DriverService> logger,
                                        IWebHostEnvironment environment,
                                        IServiceProvider serviceProvider)
        {
            _logger = logger;
            _environment = environment;
            _serviceProvider = serviceProvider;
        }

        private async Task InitializeAsync()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    // Executa migration
                    var _migrationService = scope.ServiceProvider.GetRequiredService<IMigrationService>();
                    if (_environment.EnvironmentName == "Development")
                        _migrationService.Deleted();

                    _migrationService.Execute();

                    if (_environment.EnvironmentName == "Testing")
                        _migrationService.Populate();

                    // Inicializa drivers
                    var repository = scope.ServiceProvider.GetRequiredService<IRepositorySQLite>();
                    var driveFactory = scope.ServiceProvider.GetRequiredService<IDriveFactory>();

                    var devices = await repository.Get<Device>().ToListAsync();

                    foreach (var device in devices)
                    {
                        var drive = driveFactory.GenerateDrive(device);
                        await drive.SetConfiguration(device);
                        _drives.Add(drive);
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_INITIALIZE);
                _logger.LogError(exception.Message);
            }
        }

        public async Task StartAsync()
        {
            await InitializeAsync();
            if (_drives.Any())
            {
                foreach (var driver in _drives)
                {
                    try
                    {
                        await driver.StartAsync();
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(ERROR_START);
                        _logger.LogError(exception.Message);
                    }
                }
            }
        }

        public async Task StopAsync()
        {
            if (_drives.Any())
            {
                foreach (var driver in _drives)
                {
                    try
                    {
                        await driver.StopAsync();
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(ERROR_STOP);
                        _logger.LogError(exception.Message);
                    }
                }
            }
        }

        public async Task<IDrive> GetDriveAsync(Guid id) => _drives.FirstOrDefault(x => x.Id == id);

        public async Task<IEnumerable<IDrive>> GetDrivesAsync() => _drives;

        public async Task AddDriveAsync(IDrive drive)
        {
            try
            {
                _drives.Add(drive);
                await drive.StartAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(ERROR_STOP);
                _logger.LogError(exception.Message);
            }
        }

        public async Task DeleteDriveAsync(Guid id)
        {
            var currentDrive = default(IDrive);
            try
            {
                currentDrive = _drives.FirstOrDefault(x => x.Id == id);
                if (currentDrive is not null)
                {
                    await currentDrive.StopAsync();
                    _drives.Remove(currentDrive);
                }
            }
            catch (Exception exception)
            {
                var name = currentDrive is null ? id.ToString() : currentDrive.Name;
                _logger.LogError(ERROR_DELETE.Replace(NAME, name));
                _logger.LogError(exception.Message);
            }
        }
    }
}
