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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Services
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IAuthService _authService;
        private readonly IValidator<UserViewModel> _userValidator;
        private readonly IValidator<UserInfoViewModel> _userInfoValidator;
        private readonly IValidator<UserPasswordViewModel> _userPasswordValidator;

        private const string USER_CREATED = "O usuário foi criado com sucesso";
        private const string USER_UPDATED = "O usuário foi atualizado com sucesso";
        private const string USER_DELETED = "O usuário foi deletado com sucesso";
        private const string USER_NOT_FOUND = "O usuário não foi encontrado";
        private const string USERS_NO_CONTENT = "Nenhum usuário foi encontrado";
        private const string INVALID_PASSWORD = "Senha inválida";
        private const string USERNAME_ALREADY_EXISTS = "O nome de usuário informado já existe";
        private const string CHANGE_ANOTHER_USER_DATA = "Não é permitido alterar dados de outro usuário";
        private const string CHANGE_USER_TO_ADMINISTRATOR = "Somente um administrador pode criar ou alterar um usuário administrador";
        private const string INSERT_OR_DELETE_USER_AUTHORIZATION = "Somente um administrador pode criar ou deletar um usuário";
        private const string DELETE_OR_DISABLE_ADMINISTRATOR_LIMITS = "Não é possivel deletar ou desabilitar todos os usuários administradores";

        public UserService(INotifier notifier, 
                          ILogger<UserService> logger, 
                          IMapper mapper,
                          IRepositorySQLite repository, 
                          IAuthService authService, 
                          IValidator<UserViewModel> userValidator, 
                          IValidator<UserInfoViewModel> userInfoValidator,
                          IValidator<UserPasswordViewModel> userPasswordValidator
                         ) : base(notifier, logger, mapper, repository)
        {
            _authService = authService;
            _userValidator = userValidator;
            _userInfoValidator = userInfoValidator;
            _userPasswordValidator = userPasswordValidator;
        }

        private bool CheckInsertOrDeleteAuthorization()
        {
            var loggedUser = _authService.GetLoggedUser();
            if (loggedUser.Administrator)
                return true;

            NotifyForbidden(INSERT_OR_DELETE_USER_AUTHORIZATION);
            return false;
        }

        private bool CheckUpdateUserAuthorization(UserInfoViewModel user)
        {
            var loggedUser = _authService.GetLoggedUser();

            if (loggedUser.Administrator)
                return true;

            if (user.Id == loggedUser.Id)
            {
                if(user.Administrator)
                {
                    NotifyForbidden(CHANGE_USER_TO_ADMINISTRATOR);
                    return false;
                }

                return true;
            }

            NotifyForbidden(CHANGE_ANOTHER_USER_DATA);
            return false;
        }

        private bool CheckUpdatePasswordAuthorization(Guid userId)
        {
            var loggedUser = _authService.GetLoggedUser();
            if (loggedUser.Id == userId)
                return true;

            NotifyForbidden(CHANGE_ANOTHER_USER_DATA);
            return false;
        }

        private async Task<bool> CheckForDisabledOrDeleteAdministratorAsync(Guid userId)
        {
            var exist = await _repository.Get<User>().AnyAsync(x => x.Id != userId && 
                                                                      x.Administrator && 
                                                                      x.Enabled);
            if (!exist)
                NotifyForbidden(DELETE_OR_DISABLE_ADMINISTRATOR_LIMITS);

            return exist;
        }

        private async Task<bool> CheckUserNameExistAsync(UserInfoViewModel user)
        {
            try
            {
                var result = await _repository.Get<User>()
                                              .AnyAsync(x => x.UserName.ToLower() == user.UserName.ToLower()
                                                        && x.Id != user.Id);

                return result;
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return false;
            }
        }


        public async Task<UserInfoViewModel> GetAsync(Guid id)
        {
            try
            {
                var user = await _repository.Get<User>()
                                            .FirstOrDefaultAsync(x => x.Id == id);

                if (user is null)
                    NotifyNotFound(USER_NOT_FOUND);

                return _mapper.Map<UserInfoViewModel>(user);
            }
            catch(Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public async Task<IEnumerable<UserInfoViewModel>> GetAllAsync(string name)
        {
            try
            {
                var query = _repository.Get<User>();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(u => EF.Functions.Like(u.FullName.ToLower(), $"%{name.ToLower()}%"));

                var users = await query.OrderBy(x => x.UserName)
                                       .ToListAsync();

                if (!users.Any())
                    NotifyNoContent(USERS_NO_CONTENT);

                return _mapper.Map<IEnumerable<UserInfoViewModel>>(users);
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return null;
            }
        }

        public async Task<bool> CheckUserNameExistAsync(string userName)
        {
            try
            {
                return await _repository.Get<User>()
                                        .AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
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
                var result = await _repository.Get<User>().CountAsync();
                return result;
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
                return -1;
            }
        }

        public async Task<UserInfoViewModel> InsertAsync(UserViewModel userViewModel)
        {
            try
            {
                var valid = Validate<UserViewModel>(_userValidator, userViewModel);
                if (valid)
                {
                    if (CheckInsertOrDeleteAuthorization())
                    {
                        var check = await CheckUserNameExistAsync(userViewModel.UserName);

                        if (check)
                        {
                            NotifyBadRequest(USERNAME_ALREADY_EXISTS);
                        }
                        else
                        {
                            var user = _mapper.Map<User>(userViewModel);
                            user.SetPassword(Utils.Encript(user.Password));
                            await _repository.AddAsync<User>(user);

                            NotifyCreated(user.Id.ToString(), USER_CREATED);
                            return _mapper.Map<UserInfoViewModel>(user);
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

        public async Task<UserInfoViewModel> UpdateAsync(UserInfoViewModel userInfoViewModel)
        {
            try
            {
                var valid = Validate<UserInfoViewModel>(_userInfoValidator, userInfoViewModel);
                if (valid)
                {
                    if(CheckUpdateUserAuthorization(userInfoViewModel))
                    {
                        var user = await _repository.Get<User>()
                                                    .FirstOrDefaultAsync(x => x.Id == userInfoViewModel.Id);

                        if (user is not null)
                        {
                            if (user.Administrator && (!userInfoViewModel.Administrator || !userInfoViewModel.Enabled))
                                if (!await CheckForDisabledOrDeleteAdministratorAsync(userInfoViewModel.Id))
                                    return null;

                            var check = await CheckUserNameExistAsync(userInfoViewModel);
                            if (check)
                            {
                                NotifyBadRequest(USERNAME_ALREADY_EXISTS);
                            }
                            else
                            {
                                user.SetFullName(userInfoViewModel.FullName);
                                user.SetUserName(userInfoViewModel.UserName);
                                user.SetEnabled(userInfoViewModel.Enabled);
                                user.SetAdministrator(userInfoViewModel.Administrator);

                                await _repository.EditAsync<User>(user);
                                NotifyOk(USER_UPDATED);
                                return _mapper.Map<UserInfoViewModel>(user);
                            }
                        }
                        else
                        {
                            NotifyNotFound(USER_NOT_FOUND);
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

        public async Task ChangePasswordAsync(UserPasswordViewModel userPasswordViewModel)
        {
            try
            {
                var valid = Validate<UserPasswordViewModel>(_userPasswordValidator, userPasswordViewModel);
                if (valid)
                {
                    if (CheckUpdatePasswordAuthorization(userPasswordViewModel.Id))
                    {
                        var user = await _repository.Get<User>()
                                                    .FirstOrDefaultAsync(x => x.Id == userPasswordViewModel.Id);

                        if (user.Password == Utils.Encript(userPasswordViewModel.Password))
                        {
                            user.SetPassword(Utils.Encript(userPasswordViewModel.NewPassword));
                            await _repository.EditAsync<User>(user);
                            NotifyOk(USER_UPDATED);
                        }
                        else
                        {
                            NotifyForbidden(INVALID_PASSWORD);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                NotifyInternalServerError(exception);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            if (CheckInsertOrDeleteAuthorization())
            {
                var user = await _repository.Get<User>()
                                        .FirstOrDefaultAsync(x => x.Id == id);

                if (user is null)
                {
                    NotifyNotFound(USER_NOT_FOUND);
                    return;
                }

                if (user.Administrator)
                    if (!await CheckForDisabledOrDeleteAdministratorAsync(id))
                        return;

                await _repository.DeleteAsync<User>(user);
                NotifyNoContent(USER_DELETED);
            }
        }
    }
}
