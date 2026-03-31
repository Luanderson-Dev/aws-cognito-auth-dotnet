using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using AuthService.Domain.Interfaces;
using AuthService.Domain.ValueObjects;
using AuthService.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace AuthService.Infrastructure.Repositories;

public class CognitoAuthRepository : IAuthRepository
{
    private readonly IAmazonCognitoIdentityProvider _cognitoClient;
    private readonly CognitoSettings _settings;
    
    public CognitoAuthRepository(
        IAmazonCognitoIdentityProvider cognitoClient,
        IOptions<CognitoSettings> settings)
    {
        _cognitoClient = cognitoClient;
        _settings = settings.Value;
    }
    
    public async Task<string> SignUpAsync(string email, string password, string username)
    {
        var request = new SignUpRequest
        {
            ClientId = _settings.ClientId,
            SecretHash = ComputeSecretHash(email),
            Username = email,                      
            Password = password,
            UserAttributes = new List<AttributeType>
            {
                new() { Name = "username", Value = username } 
            }
        };

        var response = await _cognitoClient.SignUpAsync(request);
        return response.UserSub;
    }
    
    public async Task ConfirmSignUpAsync(string email, string confirmationCode)
    {
        await _cognitoClient.ConfirmSignUpAsync(new ConfirmSignUpRequest
        {
            ClientId = _settings.ClientId,
            SecretHash = ComputeSecretHash(email),
            Username = email,                      
            ConfirmationCode = confirmationCode
        });
    }
    
    public async Task<AuthTokens> SignInAsync(string email, string password)
    {
        var request = new InitiateAuthRequest
        {
            ClientId = _settings.ClientId,
            AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", email },
                { "PASSWORD", password },
                { "SECRET_HASH", ComputeSecretHash(email) }
            }
        };

        var response = await _cognitoClient.InitiateAuthAsync(request);
        var tokens = response.AuthenticationResult;

        return new AuthTokens(
            tokens.AccessToken,
            tokens.IdToken,
            tokens.RefreshToken,
            tokens.ExpiresIn
        );
    }
    
    public async Task SignOutAsync(string accessToken)
    {
        await _cognitoClient.GlobalSignOutAsync(new GlobalSignOutRequest
        {
            AccessToken = accessToken
        });
    }
    
    public async Task<AuthTokens> RefreshTokenAsync(string refreshToken, string idToken)
    {
        var sub = ExtractSubFromIdToken(idToken);

        var request = new InitiateAuthRequest
        {
            ClientId = _settings.ClientId,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "REFRESH_TOKEN", refreshToken },
                { "SECRET_HASH", ComputeSecretHash(sub) } // ← sub do usuário
            }
        };

        var response = await _cognitoClient.InitiateAuthAsync(request);
        var tokens = response.AuthenticationResult;

        // Extrai o email do novo IdToken para manter consistência
        var email = ExtractEmailFromIdToken(tokens.IdToken);

        return new AuthTokens(
            tokens.AccessToken,
            tokens.IdToken,
            refreshToken,
            tokens.ExpiresIn
        );
    }

    
    public async Task ForgotPasswordAsync(string email)
    {
        await _cognitoClient.ForgotPasswordAsync(new ForgotPasswordRequest
        {
            ClientId = _settings.ClientId,
            SecretHash = ComputeSecretHash(email),
            Username = email                      
        });
    }
    
    public async Task ConfirmForgotPasswordAsync(string email, string confirmationCode, string newPassword)
    {
        await _cognitoClient.ConfirmForgotPasswordAsync(new ConfirmForgotPasswordRequest
        {
            ClientId = _settings.ClientId,
            SecretHash = ComputeSecretHash(email),
            Username = email,                      
            ConfirmationCode = confirmationCode,
            Password = newPassword
        });
    }
    
    private string ComputeSecretHash(string username)
    {
        var message = username + _settings.ClientId;
        var keyBytes = Encoding.UTF8.GetBytes(_settings.ClientSecret);
        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        return Convert.ToBase64String(hash);
    }
    
    private string ExtractSubFromIdToken(string idToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(idToken);
        return jwt.Subject; 
    }

    private string ExtractEmailFromIdToken(string idToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(idToken);
        return jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty;
    }
}