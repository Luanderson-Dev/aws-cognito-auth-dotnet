using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.UseCases.ForgotPassword;

public class ForgotPasswordHandler: IRequestHandler<ForgotPasswordCommand, ForgotPasswordResult>
{
    private readonly IAuthRepository _authRepository;
    
    public ForgotPasswordHandler(IAuthRepository authRepository) =>  
        _authRepository = authRepository;
    
    public async Task<ForgotPasswordResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        await _authRepository.ForgotPasswordAsync(request.Email);
        return new ForgotPasswordResult("If an account with the provided email exists, a password reset link has been sent.");
    }
}