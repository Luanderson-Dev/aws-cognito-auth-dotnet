namespace AuthService.Api.DTOs.Responses;

public record AuthTokensResponse(
    string AccessToken,
    string IdToken,
    string RefreshToken,
    int? ExpiresIn
);