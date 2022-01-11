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
    [Route("api/v{version:apiVersion}/user")]
    public class UserController : CustomControllerBase
    {
        private readonly IUserService _userService;

        public UserController(INotifier notifier, 
                              ILogger<AuthController> logger,
                              IUserService userService) : base(notifier, logger)
        {
            _userService = userService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoViewModel>> GetAsync(Guid id)
        {
            try
            {
                    return CustomResponse(await _userService.GetAsync(id));
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
        public async Task<ActionResult<IEnumerable<UserInfoViewModel>>> GetAllAsync(string name)
        {
            try
            {
                return CustomResponse(await _userService.GetAllAsync(name));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("check-username/{username}")]
        public async Task<ActionResult<bool?>> CheckUserNameExistAsync(string username)
        {
            try
            {
                return CustomResponse(await _userService.CheckUserNameExistAsync(username));
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
                return CustomResponse(await _userService.TotalRegistersAsync());
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
        public async Task<ActionResult> PostAsync([FromBody] UserViewModel userModelView)
        {
            try
            {
                if (userModelView.Id == Guid.Empty)
                    userModelView.Id = Guid.NewGuid();

                return CustomResponse(await _userService.InsertAsync(userModelView));
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
        public async Task<ActionResult> PutAsync(Guid id, [FromBody] UserInfoViewModel userInfoViewModel)
        {
            try
            {
                if(id != userInfoViewModel.Id)
                    return ReturnActionResultForInvalidId();

                return CustomResponse(await _userService.UpdateAsync(userInfoViewModel));
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("change-password/{id}")]
        public async Task<ActionResult> ChangePasswordAsync(Guid id, [FromBody] UserPasswordViewModel userPasswordModelView)
        {
            try
            {
                if (id != userPasswordModelView.Id)
                    return ReturnActionResultForInvalidId();
                
                await _userService.ChangePasswordAsync(userPasswordModelView);

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
                await _userService.DeleteAsync(id);

                return CustomResponse();
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }
    }
}
