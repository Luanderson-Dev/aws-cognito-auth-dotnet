using AuthService.Domain.ValueObjects;
using MediatR;

namespace AuthService.Application.UseCases.RefreshToken;

public record RefreshTokenCommand(string RefreshToken, string IdToken) : IRequest<AuthTokens>;