using Deviot.Common;
using Deviot.Hermes.Api.Bases;
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
        private readonly IDeviceService _deviceService;

        public DeviceController(INotifier notifier, 
                                ILogger<DeviceController> logger,
                                IDeviceService deviceService) : base(notifier, logger)
        {
            _deviceService = deviceService;
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
                    return CustomResponse(await _deviceService.GetAsync(id));
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
        [HttpGet("database/{id}")]
        public async Task<ActionResult<object>> GetDataAsync(Guid id)
        {
            try
            {
                return CustomResponse(await _deviceService.GetDataAsync(id));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceViewModel>>> GetAllAsync(string name = "", int take = 1000, int skip = 0)
        {
            try
            {
                return CustomResponse(await _deviceService.GetAllAsync(name, take, skip));
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
                return CustomResponse(await _deviceService.CheckNameExistAsync(name));
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
                return CustomResponse(await _deviceService.TotalRegistersAsync());
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
        public async Task<ActionResult> PostAsync([FromBody] DeviceViewModel deviceViewModel)
        {
            try
            {
                if (deviceViewModel is null)
                    deviceViewModel = new DeviceViewModel();

                if (deviceViewModel.Id == Guid.Empty)
                    deviceViewModel.Id = Guid.NewGuid();

                await _deviceService.InsertAsync(deviceViewModel);

                return CustomResponse();
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
        public async Task<ActionResult> PutAsync(Guid id, [FromBody] DeviceViewModel deviceViewModel)
        {
            try
            {
                if (deviceViewModel is null)
                    deviceViewModel = new DeviceViewModel();

                if(id != deviceViewModel.Id)
                    return ReturnActionResultForInvalidId();

                await _deviceService.UpdateAsync(deviceViewModel);

                return CustomResponse();
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
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                await _deviceService.DeleteAsync(id);

                return CustomResponse();
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        
    }
}
