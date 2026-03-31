using MediatR;

namespace AuthService.Application.UseCases.SignUp;

public record ConfirmSignUpCommand(string Email, string ConfirmationCode) : IRequest<ConfirmSignUpResult>;  
public record ConfirmSignUpResult(string Email, string Message);