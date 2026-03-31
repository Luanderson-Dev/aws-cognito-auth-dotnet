using MediatR;

namespace AuthService.Application.UseCases.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<ForgotPasswordResult>;
public record ForgotPasswordResult(string Message);