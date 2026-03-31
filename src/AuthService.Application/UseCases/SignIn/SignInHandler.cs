using AuthService.Domain.Interfaces;
using AuthService.Domain.ValueObjects;
using MediatR;

namespace AuthService.Application.UseCases.SignIn;

public class SignInHandler: IRequestHandler<SignInCommand, AuthTokens>
{
    private readonly IAuthRepository _authRepository;

    public SignInHandler(IAuthRepository authRepository)
        => _authRepository = authRepository;

    public async Task<AuthTokens> Handle(SignInCommand request, CancellationToken cancellationToken)
        => await _authRepository.SignInAsync(request.Email, request.Password);
}