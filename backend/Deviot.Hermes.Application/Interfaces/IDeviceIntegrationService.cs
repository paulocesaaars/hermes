using Deviot.Hermes.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IDeviceIntegrationService
    {
        Task StartAsync();

        Task StopAsync();

        Task AddDriveAsync(Device device);

        Task UpdateDriveAsync(Device device);

        Task DeleteDriveAsync(Guid id);

        Task<object> GetDataAsync(Guid id);
    }
}
