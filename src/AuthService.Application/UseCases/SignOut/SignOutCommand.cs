using MediatR;

namespace AuthService.Application.UseCases.SignOut;

public record SignOutCommand(string AccessToken) : IRequest<SignOutResult>;
public record SignOutResult(string Message);