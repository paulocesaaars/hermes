using AutoMapper;
using Deviot.Common;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Services
{
    public class DeviceIntegrationService : ServiceBase, IDeviceIntegrationService
    {
        private readonly IAuthService _authService;
        private readonly IDriveFactory _driveFactory;
        private readonly IDeviceService _deviceService;
        private readonly IValidator<DeviceViewModel> _deviceValidator;
        private readonly IDriverService _deviceBackgroundService;

        private const string NAME = "{name}";
        private const string DEVICE_AUTHORIZATION = "Somente administradores podem realizar essa operação";
        private const string DEVICE_NOT_FOUND = "O dispositivo não foi encontrado";

        public DeviceIntegrationService(INotifier notifier,
                                        ILogger<DeviceIntegrationService> logger,
                                        IMapper mapper,
                                        IRepositorySQLite repository,
                                        IAuthService authService,
                                        IDriveFactory driveFactory,
                                        IDeviceService deviceService,
                                        IValidator<DeviceViewModel> deviceValidator,
                                        IDriverService deviceBackgroundService
                                        ) : base(notifier, logger, mapper, repository)
        {
            _authService = authService;
            _driveFactory = driveFactory;
            _deviceService = deviceService;
            _deviceValidator = deviceValidator;
            _deviceBackgroundService = deviceBackgroundService;
        }

        private bool CheckAuthorization()
        {
            var loggedUser = _authService.GetLoggedUser();
            if (loggedUser.Administrator)
                return true;

            NotifyForbidden(DEVICE_AUTHORIZATION);
            return false;
        }

        public async Task<DeviceViewModel> GetAsync(Guid id) 
        {
            try
            {
                var device = await _deviceService.GetAsync(id);
                return _mapper.Map<DeviceViewModel>(device);
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public async Task<IEnumerable<DeviceInfoViewModel>> GetAllAsync(string name = "")
        {
            try
            {
                var devices = await _deviceService.GetAllAsync(name);
                var drives = await _deviceBackgroundService.GetDrivesAsync();

                var devicesInfoView = _mapper.Map<IEnumerable<DeviceInfoViewModel>>(devices);
                foreach (var deviceInfoView in devicesInfoView)
                    deviceInfoView.StatusConnection = drives.First(x => x.Id == deviceInfoView.Id).StatusConnection;

                return devicesInfoView;
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public async Task<bool> CheckNameExistAsync(string name) => await _deviceService.CheckNameExistAsync(name);

        public async Task<long> TotalRegistersAsync() => await _deviceService.TotalRegistersAsync();

        public async Task<DeviceViewModel> InsertAsync(DeviceViewModel deviceViewModel)
        {
            try
            {
                if(CheckAuthorization())
                {
                    if (Validate<DeviceViewModel>(_deviceValidator, deviceViewModel))
                    {
                        var device = _mapper.Map<Device>(deviceViewModel);
                        var drive = _driveFactory.GenerateDrive(device);
                        var validationResult = drive.ValidateConfiguration(device.Configuration);
                        if (validationResult.IsValid)
                        {
                            if (await _deviceService.InsertAsync(device) is null) return null;

                            await drive.SetConfiguration(device);
                            await _deviceBackgroundService.AddDriveAsync(drive);
                            return _mapper.Map<DeviceViewModel>(device);
                        }

                        foreach (var message in validationResult.Errors)
                            NotifyBadRequest(message.ErrorMessage);
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
                if (CheckAuthorization())
                {
                    if (Validate<DeviceViewModel>(_deviceValidator, deviceViewModel))
                    {
                        var device = _mapper.Map<Device>(deviceViewModel);
                        var drive = await _deviceBackgroundService.GetDriveAsync(device.Id);

                        if (drive is null)
                            NotifyNotFound(DEVICE_NOT_FOUND);

                        var validationResult = drive.ValidateConfiguration(device.Configuration);
                        if (validationResult.IsValid)
                        {
                            if (await _deviceService.UpdateAsync(device) is null)
                                return null;

                            await drive.SetConfiguration(device);
                            return _mapper.Map<DeviceViewModel>(device); ;
                        }

                        foreach (var message in validationResult.Errors)
                            NotifyBadRequest(message.ErrorMessage);
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
                    if(await _deviceService.DeleteAsync(id))
                        await _deviceBackgroundService.DeleteDriveAsync(id);
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
                var drive = await _deviceBackgroundService.GetDriveAsync(id);

                if (drive is null)
                    NotifyNotFound(DEVICE_NOT_FOUND);

                return await drive.GetDataAsync();
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public async Task SetDataAsync(Guid id, object data)
        {
            try
            {
                if (CheckAuthorization())
                {
                    var drive = await _deviceBackgroundService.GetDriveAsync(id);

                    if (drive is null)
                        NotifyNotFound(DEVICE_NOT_FOUND);

                    var jsonData = Utils.Serializer(data);

                    var validationResult = drive.ValidateWriteData(jsonData);

                    if(!validationResult.IsValid)
                        foreach (var message in validationResult.Errors)
                            NotifyBadRequest(message.ErrorMessage);

                    await drive.SetDataAsync(jsonData);
                }
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }
        }
    }
}
