using AutoMapper;
using Deviot.Common;
using Deviot.Hermes.Application.Bases;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Application.ViewModels;
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
        private readonly IAuthService _authService;
        private readonly IDeviceIntegrationService _deviceIntegrationService;
        private readonly IValidator<DeviceViewModel> _deviceValidator;
        private readonly IDeviceValidationService _deviceValidationService;

        private const string DEVICE_CREATED = "O dispositivo foi criado com sucesso";
        private const string DEVICE_UPDATED = "O dispositivo foi atualizado com sucesso";
        private const string DEVICE_DELETED = "O dispositivo foi deletado com sucesso";
        private const string DEVICES_NO_CONTENT = "Nenhum dispositivo foi encontrado";
        private const string DEVICE_NOT_FOUND = "O dispositivo não foi encontrado";
        private const string DEVICE_NAME_ALREADY_EXISTS = "O nome do dispositivo informado já existe";
        private const string DEVICE_AUTHORIZATION = "Somente um administrador pode criar, editar ou deletar um dispositivo";

        public DeviceService(INotifier notifier,
                          ILogger<DeviceService> logger,
                          IMapper mapper,
                          IRepositorySQLite repository,
                          IAuthService authService,
                          IDeviceIntegrationService deviceIntegrationService,
                          IValidator<DeviceViewModel> deviceValidator,
                          IDeviceValidationService deviceValidationService
                         ) : base(notifier, logger, mapper, repository)
        {
            _authService = authService;
            _deviceIntegrationService = deviceIntegrationService;
            _deviceValidator = deviceValidator;
            _deviceValidationService = deviceValidationService;
        }

        private bool CheckAuthorization()
        {
            var loggedUser = _authService.GetLoggedUser();
            if (loggedUser.Administrator)
                return true;

            NotifyForbidden(DEVICE_AUTHORIZATION);
            return false;
        }

        private async Task<bool> CheckNameExistAsync(DeviceViewModel device)
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

        private bool ValidateConfiguration(Device device)
        {
            var result = _deviceValidationService.Validate(device);

            if (result.IsValid)
                return true;

            foreach (var message in result.Errors)
                NotifyBadRequest(message.ErrorMessage);

            return false;
        }

        public async Task<DeviceViewModel> GetAsync(Guid id)
        {
            try
            {
                var device = await _repository.Get<Device>()
                                              .FirstOrDefaultAsync(x => x.Id == id);

                if (device is null)
                    NotifyNotFound(DEVICE_NOT_FOUND);

                return _mapper.Map<DeviceViewModel>(device);
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public async Task<IEnumerable<DeviceViewModel>> GetAllAsync(string name = "")
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

                return _mapper.Map<IEnumerable<DeviceViewModel>>(devices);
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

        public async Task<DeviceViewModel> InsertAsync(DeviceViewModel deviceViewModel) 
        {
            try
            {
                if (Validate<DeviceViewModel>(_deviceValidator, deviceViewModel))
                {
                    var device = _mapper.Map<Device>(deviceViewModel);
                    if (ValidateConfiguration(device) && CheckAuthorization())
                    {
                        var check = await CheckNameExistAsync(deviceViewModel.Name);
                        if (check)
                        {
                            NotifyBadRequest(DEVICE_NAME_ALREADY_EXISTS);
                        }
                        else
                        {
                            await _deviceIntegrationService.AddDriveAsync(device);
                            await _repository.AddAsync<Device>(device);
                            NotifyCreated(device.Id.ToString(), DEVICE_CREATED);

                            return _mapper.Map<DeviceViewModel>(device);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }

            return null;
        }

        public async Task<DeviceViewModel> UpdateAsync(DeviceViewModel deviceViewModel)
        {
            try
            {
                if (Validate<DeviceViewModel>(_deviceValidator, deviceViewModel))
                {
                    var device = _mapper.Map<Device>(deviceViewModel);
                    if (ValidateConfiguration(device) && CheckAuthorization())
                    {
                        var currentDevice = await _repository.Get<Device>()
                                                      .FirstOrDefaultAsync(x => x.Id == deviceViewModel.Id);

                        if (currentDevice is not null)
                        {
                            var check = await CheckNameExistAsync(deviceViewModel);
                            if (check)
                            {
                                NotifyBadRequest(DEVICE_NAME_ALREADY_EXISTS);
                            }
                            else
                            {
                                currentDevice.SetName(device.Name);
                                currentDevice.SetEnable(device.Enable);
                                currentDevice.SetConfiguration(device.Configuration);

                                await _deviceIntegrationService.UpdateDriveAsync(currentDevice);
                                await _repository.EditAsync<Device>(currentDevice);
                                NotifyOk(DEVICE_UPDATED);

                                return _mapper.Map<DeviceViewModel>(currentDevice);
                            }
                        }
                        else
                        {
                            NotifyNotFound(DEVICE_NOT_FOUND);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }

            return null;
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                if (CheckAuthorization())
                {
                    var device = await _repository.Get<Device>()
                                                  .FirstOrDefaultAsync(x => x.Id == id);

                    if (device is null)
                    {
                        NotifyNotFound(DEVICE_NOT_FOUND);
                        return;
                    }

                    await _deviceIntegrationService.DeleteDriveAsync(id);
                    await _repository.DeleteAsync<Device>(device);
                    NotifyOk(DEVICE_DELETED);
                }
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }
        }

        public async Task<object> GetDataAsync(Guid id)
        {
            try
            {
                return await _deviceIntegrationService.GetDataAsync(id);
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }

            return null;
        }

        public async Task WriteDataAsync(object value)
        {
            try
            {
                await _deviceIntegrationService.WriteDataAsync(value);
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }
        }
    }
}
