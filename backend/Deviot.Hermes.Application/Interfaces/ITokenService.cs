using Deviot.Hermes.Application.ViewModels;

namespace Deviot.Hermes.Application.Interfaces
{
    public interface ITokenService
    {
        TokenViewModel GenerateToken(UserInfoViewModel user);
    }
}
