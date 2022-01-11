using Deviot.Common;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deviot.Hermes.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/device")]
    public class DeviceController : CustomControllerBase
    {
        private readonly IDeviceIntegrationService _deviceIntegrationService;

        public DeviceController(INotifier notifier, 
                                ILogger<DeviceController> logger,
                                IDeviceIntegrationService deviceIntegrationService) : base(notifier, logger)
        {
            _deviceIntegrationService = deviceIntegrationService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceViewModel>> GetAsync(Guid id)
        {
            try
            {
                    return CustomResponse(await _deviceIntegrationService.GetAsync(id));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceViewModel>>> GetAllAsync(string name)
        {
            try
            {
                return CustomResponse(await _deviceIntegrationService.GetAllAsync(name));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("data/{id}")]
        public async Task<ActionResult<object>> GetDataAsync(Guid id)
        {
            try
            {
                return CustomResponse(await _deviceIntegrationService.GetDataAsync(id));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("check-name/{name}")]
        public async Task<ActionResult<bool?>> CheckNameExistAsync(string name)
        {
            try
            {
                return CustomResponse(await _deviceIntegrationService.CheckNameExistAsync(name));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("total-registers")]
        public async Task<ActionResult<long?>> TotalRegistersAsync()
        {
            try
            {
                return CustomResponse(await _deviceIntegrationService.TotalRegistersAsync());
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<ActionResult<DeviceViewModel>> PostAsync([FromBody] DeviceViewModel deviceViewModel)
        {
            try
            {
                if (deviceViewModel.Id == Guid.Empty)
                    deviceViewModel.Id = Guid.NewGuid();

                return CustomResponse(await _deviceIntegrationService.InsertAsync(deviceViewModel));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]
        public async Task<ActionResult<DeviceViewModel>> PutAsync(Guid id, [FromBody] DeviceViewModel deviceViewModel)
        {
            try
            {
                if (deviceViewModel is null)
                    deviceViewModel = new DeviceViewModel();

                if (id != deviceViewModel.Id)
                    return ReturnActionResultForInvalidId();

                return CustomResponse(await _deviceIntegrationService.UpdateAsync(deviceViewModel));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("data/{id}")]
        public async Task<ActionResult<DeviceViewModel>> PutAsync(Guid id, [FromBody] object data)
        {
            try
            {
                await _deviceIntegrationService.SetDataAsync(id, data);
                return CustomResponse();
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _deviceIntegrationService.DeleteAsync(id);

                return CustomResponse();
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }
    }
}
