using Deviot.Hermes.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserInfoViewModel> GetAsync(Guid id);

        Task<IEnumerable<UserInfoViewModel>> GetAllAsync(string name = "", int take = 1000, int skip = 0);

        Task<bool> CheckUserNameExistAsync(string userName);

        Task<long> TotalRegistersAsync();

        Task InsertAsync(UserViewModel userViewModel);

        Task UpdateAsync(UserInfoViewModel userInfoViewModel);

        Task ChangePasswordAsync(UserPasswordViewModel userPasswordViewModel);

        Task DeleteAsync(Guid id);
    }
}
