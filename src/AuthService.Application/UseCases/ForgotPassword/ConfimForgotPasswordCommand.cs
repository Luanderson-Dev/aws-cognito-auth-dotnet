using MediatR;

namespace AuthService.Application.UseCases.ForgotPassword;

public record ConfirmForgotPasswordCommand (string Email, string ConfirmationCode, string NewPassword) : IRequest<ConfirmForgotPasswordResult>;
public record ConfirmForgotPasswordResult(string Message);