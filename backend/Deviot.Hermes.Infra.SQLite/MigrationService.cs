using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Infra.SQLite.Configuration;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using Deviot.Hermes.Infra.SQLite.TestData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Deviot.Hermes.Infra.SQLite
{
    [ExcludeFromCodeCoverage]
    public class MigrationService : IMigrationService
    {
        private readonly ILogger _logger;
        private readonly IRepositorySQLite _repository;
        private readonly ApplicationDbContext _applicationDbContext;

        private const string MIGRATION_EXECUTE_ERROR = "Não foi possível executar a migration";
        private const string MIGRATION_DELETED_ERROR = "Não foi possível deletar o banco de dados";
        private const string MIGRATION_POPULATE_ERROR = "Não foi possível popular o banco de dados";


        public MigrationService(ApplicationDbContext applicationDbContext, ILogger<MigrationService> logger, IRepositorySQLite repository)
        {
            _logger = logger;
            _repository = repository;
            _applicationDbContext = applicationDbContext;
        }

        private async Task PopulateUsersAsync()
        {
            var currentUsers = await _repository.Get<User>().ToListAsync();
            var users = UserData.GetUsers();

            // Expurge
            foreach (var user in currentUsers)
                await _repository.DeleteAsync<User>(user);

            currentUsers = await _repository.Get<User>().ToListAsync();

            foreach (var user in users)
                await _repository.AddAsync<User>(user);
        }

        private async Task PopulateDevicesAsync()
        {
            var currentDevices = await _repository.Get<Device>().ToListAsync();
            var devices = DeviceData.GetDevices();

            // Expurge
            foreach (var device in currentDevices)
                await _repository.DeleteAsync<Device>(device);

            foreach (var device in devices)
                await _repository.AddAsync<Device>(device);
        }

        public void Execute()
        {
            try
            {
                _applicationDbContext.Database.Migrate();
            }
            catch (Exception exception)
            {
                _logger.LogError(MIGRATION_EXECUTE_ERROR);
                _logger.LogError(exception.Message);
            }
        }

        public void Deleted()
        {
            try
            {
                _applicationDbContext.Database.EnsureDeleted();
            }
            catch (Exception exception)
            {
                _logger.LogError(MIGRATION_DELETED_ERROR);
                _logger.LogError(exception.Message);
            }
            
        }

        public void Populate()
        {
            try
            {
                var tasks = new List<Task>();
                tasks.Add(PopulateUsersAsync());
                tasks.Add(PopulateDevicesAsync());
                var task = Task.WhenAll(tasks);
                task.Wait();
            }
            catch (Exception exception)
            {
                _logger.LogError(MIGRATION_POPULATE_ERROR);
                _logger.LogError(exception.Message);
            }
        }
    }
}
