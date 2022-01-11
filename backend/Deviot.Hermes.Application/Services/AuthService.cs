using AutoMapper;
using Deviot.Common;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Application.ViewModels;
using Deviot.Hermes.Domain.Entities;
using Deviot.Hermes.Infra.SQLite.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Services
{
    public class AuthService : ServiceBase, IAuthService
    {
        private readonly ITokenService _tokenService;

        private readonly IValidator<LoginViewModel> _loginValidator;

        private UserInfoViewModel _userLogged;

        private const string NOTFOUND_USER_ERROR = "Usuário ou senha inválidos";

        public bool IsAuthenticated { get; private set; }

        public AuthService(INotifier notifier, 
                           ILogger<AuthService> logger,
                           IMapper mapper,
                           IRepositorySQLite repository, 
                           ITokenService tokenService,
                           IValidator<LoginViewModel> loginValidator
                          ) : base (notifier, logger, mapper, repository)
        {
            _tokenService = tokenService;
            _loginValidator = loginValidator;

            IsAuthenticated = false;
        }

        public async Task<TokenViewModel> LoginAsync(LoginViewModel login)
        {
            try
            {
                var result = Validate<LoginViewModel>(_loginValidator, login);
                if (result)
                {
                    var user = await _repository.Get<User>()
                                                .FirstOrDefaultAsync(x => x.Enabled
                                                    && x.UserName.ToLower() == login.UserName.ToLower()
                                                    && x.Password == Utils.Encript(login.Password));

                    if (user is not null)
                    {
                        var userInfoViewModel = _mapper.Map<UserInfoViewModel>(user);
                        return _tokenService.GenerateToken(userInfoViewModel);
                    }

                    _notifier.Notify(HttpStatusCode.NotFound, NOTFOUND_USER_ERROR);
                }

                return null;
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public UserInfoViewModel GetLoggedUser() => _userLogged;

        public void SetLoggedUser(UserInfoViewModel user)
        {
            if(IsAuthenticated is false)
            {
                IsAuthenticated = true;
                _userLogged = user;
            }
        }
    }
}
