using AuthService.Domain.ValueObjects;
using MediatR;

namespace AuthService.Application.UseCases.SignIn;

public record SignInCommand(string Email, string Password) : IRequest<AuthTokens>;