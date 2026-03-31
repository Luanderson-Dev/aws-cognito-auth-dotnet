using AuthService.Domain.ValueObjects;

namespace AuthService.Domain.Interfaces;

public interface IAuthRepository
{
    Task<string> SignUpAsync(string email, string password, string username);
    Task ConfirmSignUpAsync(string email, string confirmationCode);
    Task<AuthTokens> SignInAsync(string email, string password);
    Task SignOutAsync(string accessToken);
    Task<AuthTokens> RefreshTokenAsync(string refreshToken, string idToken);
    Task ForgotPasswordAsync(string email);
    Task ConfirmForgotPasswordAsync(string email, string confirmationCode, string newPassword);
}