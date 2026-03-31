using MediatR;

namespace AuthService.Application.UseCases.SignUp;

public record SignUpCommand(string Email, string Password, string Username) : IRequest<SignUpResult>;
public record SignUpResult(string UserId, string Message);