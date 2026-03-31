using AuthService.Application.UseCases.ForgotPassword;
using AuthService.Application.UseCases.SignIn;
using AuthService.Application.UseCases.SignOut;
using AuthService.Application.UseCases.SignUp;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;
    
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("confirm-signup")]
    public async Task<IActionResult> Confirm([FromBody] ConfirmSignUpCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var tokens = await _mediator.Send(command);
        return Ok(tokens);
    }
    
    [HttpPost("signout")]
    public async Task<IActionResult> SignOut([FromBody] SignOutCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
    
    [HttpPost("confirm-forgot-password")]
    public async Task<IActionResult> ConfirmForgotPassword([FromBody] ConfirmForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}