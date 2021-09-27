using Deviot.Hermes.Application.ViewModels;
using System.Threading.Tasks;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }

        UserInfoViewModel GetLoggedUser();

        void SetLoggedUser(UserInfoViewModel user);

        Task<TokenViewModel> LoginAsync(LoginViewModel login);
    }
}
