using Deviot.Common;
using Deviot.Hermes.Application.Configurations;
using Deviot.Hermes.Application.Interfaces;
using Deviot.Hermes.Application.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Deviot.Hermes.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        private const string CONFIG_ERROR = "As configurações do token não foram informadas";
        private const string TOKEN_ERROR = "Erro ao gerar o token";

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            if (jwtSettings.Value is null)
                throw new Exception(CONFIG_ERROR);
            
            _jwtSettings = jwtSettings.Value;
        }

        private static ClaimsIdentity GenerateClaimsIdentity(UserInfoViewModel user)
        {
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));

            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Nbf, Utils.ToUnixEpochDate(DateTime.UtcNow).ToString()));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.Iat, Utils.ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            identityClaims.AddClaim(new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName));
            identityClaims.AddClaim(new Claim("user-id", user.Id.ToString()));
            identityClaims.AddClaim(new Claim("user-fullname", user.FullName.ToString()));
            identityClaims.AddClaim(new Claim("user-username", user.UserName.ToString()));
            identityClaims.AddClaim(new Claim("user-administrator", user.Administrator.ToString()));

            return identityClaims;
        }

        private string GenerateAccessToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.ValidIssuer,
                Audience = _jwtSettings.ValidAudience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.ExpirationTimeSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        public TokenViewModel GenerateToken(UserInfoViewModel user)
        {
            try
            {
                var identityClaims = GenerateClaimsIdentity(user);
                var accessToken = GenerateAccessToken(identityClaims);
                return new TokenViewModel { AccessToken = accessToken, User = user };
            }
            catch (Exception exception)
            {
                throw new Exception(TOKEN_ERROR, exception);
            }
        }
    }
}
