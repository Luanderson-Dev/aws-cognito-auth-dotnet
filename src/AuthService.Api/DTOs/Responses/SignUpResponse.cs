namespace AuthService.Api.DTOs.Responses;

public record SignUpResponse(
    string UserId,
    string Message
);