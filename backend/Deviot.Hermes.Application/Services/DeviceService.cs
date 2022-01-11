using AutoMapper;
using Deviot.Common;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Services
{
    public class DeviceService : ServiceBase, IDeviceService
    {
        private const string DEVICE_CREATED = "O dispositivo foi criado com sucesso";
        private const string DEVICE_UPDATED = "O dispositivo foi atualizado com sucesso";
        private const string DEVICE_DELETED = "O dispositivo foi deletado com sucesso";
        private const string DEVICES_NO_CONTENT = "Nenhum dispositivo foi encontrado";
        private const string DEVICE_NOT_FOUND = "O dispositivo não foi encontrado";
        private const string DEVICE_NAME_ALREADY_EXISTS = "O nome do dispositivo informado já existe";

        public DeviceService(INotifier notifier,
                             ILogger<DeviceService> logger,
                             IMapper mapper,
                             IRepositorySQLite repository
                            ) : base(notifier, logger, mapper, repository) {}

        private async Task<bool> CheckNameExistAsync(Device device)
        {
            try
            {
                var result = await _repository.Get<Device>()
                                              .AnyAsync(x => x.Name.ToLower() == device.Name.ToLower() &&
                                                             x.Id != device.Id);

                return result;
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return false;
            }
        }

        public async Task<Device> GetAsync(Guid id)
        {
            try
            {
                var device = await _repository.Get<Device>()
                                              .FirstOrDefaultAsync(x => x.Id == id);

                if (device is null)
                    NotifyNotFound(DEVICE_NOT_FOUND);

                return device;
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public async Task<IEnumerable<Device>> GetAllAsync(string name)
        {
            try
            {
                var query = _repository.Get<Device>();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(u => EF.Functions.Like(u.Name.ToLower(), $"%{name.ToLower()}%"));

                var devices = await query.OrderBy(x => x.Name)
                                         .ToListAsync();

                if (!devices.Any())
                    NotifyNoContent(DEVICES_NO_CONTENT);

                return devices;
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public async Task<bool> CheckNameExistAsync(string name)
        {
            try
            {
                return await _repository.Get<Device>()
                                        .AnyAsync(x => x.Name.ToLower() == name.ToLower());
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return false;
            }
        }

        public async Task<long> TotalRegistersAsync()
        {
            try
            {
                var result = await _repository.Get<Device>().CountAsync();
                return result;
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return -1;
            }
        }

        public async Task<Device> InsertAsync(Device device) 
        {
            try
            {
                var check = await CheckNameExistAsync(device.Name);
                if (check)
                {
                    NotifyBadRequest(DEVICE_NAME_ALREADY_EXISTS);
                }
                else
                {
                    await _repository.AddAsync<Device>(device);
                    NotifyCreated(device.Id.ToString(), DEVICE_CREATED);
                    return device;
                }
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }

            return null;
        }

        public async Task<Device> UpdateAsync(Device device)
        {
            try
            {
                var currentDevice = await _repository.Get<Device>()
                                                     .FirstOrDefaultAsync(x => x.Id == device.Id);

                if (currentDevice is not null)
                {
                    var check = await CheckNameExistAsync(device);
                    if (check)
                    {
                        NotifyBadRequest(DEVICE_NAME_ALREADY_EXISTS);
                    }
                    else
                    {
                        currentDevice.SetName(device.Name);
                        currentDevice.SetEnable(device.Enabled);
                        currentDevice.SetConfiguration(device.Configuration);

                        await _repository.EditAsync<Device>(currentDevice);
                        NotifyOk(DEVICE_UPDATED);
                        return device;
                    }
                }
                else
                {
                    NotifyNotFound(DEVICE_NOT_FOUND);
                }
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var device = await _repository.Get<Device>()
                                              .FirstOrDefaultAsync(x => x.Id == id);

                if (device is not null)
                {
                    await _repository.DeleteAsync<Device>(device);
                    NotifyOk(DEVICE_DELETED);
                    return true;
                }

                NotifyNotFound(DEVICE_NOT_FOUND);
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }

            return false;
        }
    }
}
