using Application.Auth.AuthCommands.AuthCommandRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class AuthController(IMediator mediator) : BaseController()
{
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterAuthCommand request)
    {
        var result = await mediator.Send(request);
        return HandleResult(result);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginAuthCommand request)
    {
        var result = await mediator.Send(request);
        return HandleResult(result);
    }
}
