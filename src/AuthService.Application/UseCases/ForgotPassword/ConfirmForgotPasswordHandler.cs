using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.UseCases.ForgotPassword;

public class ConfirmForgotPasswordHandler : IRequestHandler<ConfirmForgotPasswordCommand, ConfirmForgotPasswordResult>
{
    private readonly IAuthRepository _authRepository;
    
    public ConfirmForgotPasswordHandler(IAuthRepository authRepository) =>
        _authRepository = authRepository;
    
    public async Task<ConfirmForgotPasswordResult> Handle(ConfirmForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        await _authRepository.ConfirmForgotPasswordAsync(request.Email, request.ConfirmationCode, request.NewPassword);
        return new ConfirmForgotPasswordResult("Password reset successful.");
    }
}