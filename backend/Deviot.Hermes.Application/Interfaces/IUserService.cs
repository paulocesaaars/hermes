using Deviot.Hermes.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserInfoViewModel> GetAsync(Guid id);

        Task<IEnumerable<UserInfoViewModel>> GetAllAsync(string name);

        Task<bool> CheckUserNameExistAsync(string userName);

        Task<long> TotalRegistersAsync();

        Task<UserInfoViewModel> InsertAsync(UserViewModel userViewModel);

        Task<UserInfoViewModel> UpdateAsync(UserInfoViewModel userInfoViewModel);

        Task ChangePasswordAsync(UserPasswordViewModel userPasswordViewModel);

        Task DeleteAsync(Guid id);
    }
}
