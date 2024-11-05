using FgssrApi.Dtos;

namespace FgssrApi.Services
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto registerdto);
        Task<AuthDto> LoginAsync(LoginDto logindto);
        Task<bool> LogoutAsync(string refreshToken);

        Task<ChangePasswordDto> ChangePasswordAsync(ChangePasswordDto cpd, string userName);

        Task<ChangePhoneNumberDto> ChangePhoneNumberAsync(ChangePhoneNumberDto cpnd, string userName);

        Task<ChangeEmailDto> ChangeEmailAsync(ChangeEmailDto ced, string? userName);

        Task<string> CreateVerifyEmailAsync(string? userName);

        Task<string> ConfirmVerifyEmailAsync(string userName, string? verifyCode);

        Task<UserProfileDto> GetUserDataAsync(string userName);
        Task<UserProfileDto> UpdateUserDataAsync(UserProfileDto upd , string userName);


    }
}
