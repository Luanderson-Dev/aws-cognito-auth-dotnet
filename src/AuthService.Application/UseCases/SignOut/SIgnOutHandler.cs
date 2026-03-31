using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.UseCases.SignOut;

public class SIgnOutHandler: IRequestHandler<SignOutCommand, SignOutResult>
{
    private readonly IAuthRepository _authRepository;
    
    public SIgnOutHandler(IAuthRepository authRepository) =>
        _authRepository = authRepository;
    
    public async Task<SignOutResult> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _authRepository.SignOutAsync(request.AccessToken);
        return new SignOutResult("Successfully signed out.");
    }
}