using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.UseCases.SignUp;

public class ConfirmSignUpHandler : IRequestHandler<ConfirmSignUpCommand, ConfirmSignUpResult>
{
    private readonly IAuthRepository _authRepository;

    public ConfirmSignUpHandler(IAuthRepository authRepository) =>
        _authRepository = authRepository;
    
    public async Task<ConfirmSignUpResult> Handle(ConfirmSignUpCommand request, CancellationToken cancellationToken)
    {
        await _authRepository.ConfirmSignUpAsync(
            request.Email,
            request.ConfirmationCode
        );

        return new ConfirmSignUpResult(request.Email,"Email confirmed successfully.");
    }
}