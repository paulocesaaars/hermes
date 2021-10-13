using Deviot.Hermes.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IDeviceService
    {
        Task<DeviceViewModel> GetAsync(Guid id);

        Task<IEnumerable<DeviceViewModel>> GetAllAsync(string name = "", int take = 1000, int skip = 0);

        Task<bool> CheckNameExistAsync(string name);

        Task<long> TotalRegistersAsync();

        Task InsertAsync(DeviceViewModel deviceViewModel);

        Task UpdateAsync(DeviceViewModel deviceViewModel);

        Task DeleteAsync(Guid id);

        Task<object> GetDataAsync(Guid id);
    }
}
