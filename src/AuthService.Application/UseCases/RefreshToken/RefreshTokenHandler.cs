using AuthService.Domain.Interfaces;
using AuthService.Domain.ValueObjects;
using MediatR;

namespace AuthService.Application.UseCases.RefreshToken;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, AuthTokens>
{
    private readonly IAuthRepository _authRepository;

    public RefreshTokenHandler(IAuthRepository authRepository)
        => _authRepository = authRepository;

    public async Task<AuthTokens> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        => await _authRepository.RefreshTokenAsync(request.RefreshToken, request.IdToken);
}