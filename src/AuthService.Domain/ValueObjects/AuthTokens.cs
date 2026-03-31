namespace AuthService.Domain.ValueObjects;

public record AuthTokens(
    string AccessToken,
    string IdToken,
    string RefreshToken,
    int? ExpiresIn
);