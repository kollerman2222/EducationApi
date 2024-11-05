using FgssrApi.Dtos;
using FgssrApi.Models;

namespace FgssrApi.Services
{
    public interface ITokenService
    {
        string CreateJwtAccessToken(ApplicationUser user);

        Task<string> CreateJwtRefreshToken(ApplicationUser user, string jwtAccessToken);


        Task<ValidateTokenDto> ValidateTokenAndUpdateAsync(string accessToken, string refreshToken);
    }
}
