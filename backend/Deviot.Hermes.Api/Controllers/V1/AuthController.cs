using AutoMapper;
using Deviot.Common;
using Deviot.Hermes.Api.Bases;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Deviot.Hermes.Api.Controllers.V1
{
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : CustomControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(INotifier notifier, 
                              ILogger<AuthController> logger, 
                              IAuthService authService) : base(notifier, logger)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<ActionResult<TokenViewModel>> LoginAsync(LoginViewModel loginModelView)
        {
            try
            {
                return CustomResponse(await _authService.LoginAsync(loginModelView));
            }
            catch (Exception exception)
            {
                return ReturnActionResultForGenericError(exception);
            }
        }
    }
}
