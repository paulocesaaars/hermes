using Deviot.Hermes.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IDeviceIntegrationService
    {
        Task<DeviceViewModel> GetAsync(Guid id);

        Task<IEnumerable<DeviceInfoViewModel>> GetAllAsync(string name = "");

        Task<bool> CheckNameExistAsync(string name);

        Task<long> TotalRegistersAsync();

        Task<DeviceViewModel> InsertAsync(DeviceViewModel deviceViewModel);

        Task<DeviceViewModel> UpdateAsync(DeviceViewModel deviceViewModel);

        Task DeleteAsync(Guid id);

        Task<object> GetDataAsync(Guid id);

        Task SetDataAsync(Guid id, object data);
    }
}
