using AuthService.Api.DTOs.Responses;
using AuthService.Application.UseCases.ForgotPassword;
using AuthService.Application.UseCases.RefreshToken;
using AuthService.Application.UseCases.SignIn;
using AuthService.Application.UseCases.SignOut;
using AuthService.Application.UseCases.SignUp;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("signup")]
    [SwaggerOperation(Summary = "Register a new user",
        Description = "A confirmation code will be sent to the provided email.")]
    [ProducesResponseType(typeof(SignUpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new SignUpResponse(result.UserId, result.Message));
    }

    [HttpPost("confirm-signup")]
    [SwaggerOperation(Summary = "Confirm account via email code", Description = "Code expires in 24 hours.")]
    [ProducesResponseType(typeof(ConfirmSignUpResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Confirm([FromBody] ConfirmSignUpCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new ConfirmSignUpResponse(result.Email, result.Message));
    }

    [HttpPost("signin")]
    [SwaggerOperation(Summary = "Sign in and receive JWT tokens",
        Description = "AccessToken expires in 1 hour. RefreshToken expires in 30 days.")]
    [ProducesResponseType(typeof(AuthTokensResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var tokens = await _mediator.Send(command);
        return Ok(new AuthTokensResponse(
            tokens.AccessToken,
            tokens.IdToken,
            tokens.RefreshToken,
            tokens.ExpiresIn
        ));
    }

    [HttpPost("signout")]
    [SwaggerOperation(Summary = "Revoke all active sessions", Description = "Requires a valid AccessToken.")]
    [ProducesResponseType(typeof(SignOutResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignOut([FromBody] SignOutCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new SignOutResponse(result.Message));
    }

    [HttpPost("forgot-password")]
    [SwaggerOperation(Summary = "Request a password reset code", Description = "Reset code sent to email. Expires in 1 hour.")]
    [ProducesResponseType(typeof(ForgotPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new ForgotPasswordResponse(result.Message));
    }

    [HttpPost("confirm-forgot-password")]
    [SwaggerOperation(Summary = "Reset password with confirmation code", Description = "Use the code received via forgot-password. Expires in 1 hour.")]
    [ProducesResponseType(typeof(ConfirmForgotPasswordResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmForgotPassword([FromBody] ConfirmForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(new ConfirmForgotPasswordResponse(result.Message));
    }

    [HttpPost("refresh-token")]
    [SwaggerOperation(Summary = "Issue a new AccessToken",
        Description = "Requires the RefreshToken and IdToken from the last sign-in.")]
    [ProducesResponseType(typeof(AuthTokensResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var tokens = await _mediator.Send(command);
        return Ok(new AuthTokensResponse(
            tokens.AccessToken,
            tokens.IdToken,
            tokens.RefreshToken,
            tokens.ExpiresIn
        ));
    }
}