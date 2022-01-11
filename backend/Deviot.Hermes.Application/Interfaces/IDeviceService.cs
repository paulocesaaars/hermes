using Deviot.Hermes.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IDeviceService
    {
        Task<Device> GetAsync(Guid id);

        Task<IEnumerable<Device>> GetAllAsync(string name = "");

        Task<bool> CheckNameExistAsync(string name);

        Task<long> TotalRegistersAsync();

        Task<Device> InsertAsync(Device device);

        Task<Device> UpdateAsync(Device device);

        Task<bool> DeleteAsync(Guid id);
    }
}
