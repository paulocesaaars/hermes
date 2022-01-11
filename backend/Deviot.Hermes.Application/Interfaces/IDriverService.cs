using Deviot.Hermes.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IDriverService
    {
        Task StartAsync();

        Task StopAsync();

        Task<IDrive> GetDriveAsync(Guid id);

        Task<IEnumerable<IDrive>> GetDrivesAsync();

        Task AddDriveAsync(IDrive drive);

        Task DeleteDriveAsync(Guid id);
    }
}
