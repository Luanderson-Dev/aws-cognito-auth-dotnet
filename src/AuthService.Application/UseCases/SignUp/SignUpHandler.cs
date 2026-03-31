using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.UseCases.SignUp;

public class SignUpHandler: IRequestHandler<SignUpCommand, SignUpResult>
{
    private readonly IAuthRepository _authRepository;
    
    public SignUpHandler(IAuthRepository authRepository)
        => _authRepository = authRepository;
    
    public async Task<SignUpResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var userId = await _authRepository.SignUpAsync(
            request.Email,
            request.Password,
            request.Username
        );

        return new SignUpResult(userId, "User registered. Check your email for the confirmation code.");
    }
}