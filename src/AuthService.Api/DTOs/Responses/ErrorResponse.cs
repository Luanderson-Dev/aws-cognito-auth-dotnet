namespace AuthService.Api.DTOs.Responses;

public record ErrorResponse(
    string Error,
    string Message
);