using Deviot.Common;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Application.ViewModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;

namespace Deviot.Hermes.Api.Filters
{
    [ExcludeFromCodeCoverage]

    public class ControllerActionFilter : IActionFilter
    {
        private readonly IAuthService _authService;
        private readonly ILogger _logger;

        private const string GET_LOGGED_USER_ERROR = "Houve um problema ao buscar as informações do usuário logado";
        private const string INTERNAL_ERROR_MESSAGE = "A requisição não foi executada, houve um erro interno";

        public ControllerActionFilter(IAuthService authService, ILogger<ControllerActionFilter> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        private UserInfoViewModel GetLoggedUser(IEnumerable<Claim> claims)
        {
            try
            {
                var id = new Guid(claims.First(x => x.Type.ToLower() == "user-id").Value);
                var fullname = claims.First(x => x.Type.ToLower() == "user-fullname").Value;
                var username = claims.First(x => x.Type.ToLower() == "user-username").Value;
                var administrator = bool.Parse(claims.First(x => x.Type.ToLower() == "user-administrator").Value);

                return new UserInfoViewModel { 
                    Id = id, 
                    FullName = fullname,
                    UserName = username,
                    Enabled = true,
                    Administrator = administrator 
                };
            }
            catch (Exception exception)
            {
                var errors = Utils.GetAllExceptionMessages(exception);
                foreach (var error in errors)
                    _logger.LogError(error);

                throw new Exception(GET_LOGGED_USER_ERROR);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                if (context.HttpContext.User.Identity.IsAuthenticated)
                {
                    var user = GetLoggedUser(context.HttpContext.User.Claims);
                    _authService.SetLoggedUser(user);
                }
            }
            catch (Exception exception)
            {
                var errors = Utils.GetAllExceptionMessages(exception);
                foreach (var error in errors)
                    _logger.LogError(error);

                throw new Exception(INTERNAL_ERROR_MESSAGE);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
