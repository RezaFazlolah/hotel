using Api.DTOs.AuthDtos;
using Application.Commands.AuthCommands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[AllowAnonymous]
public class AuthController(IMediator mediator, IMapper mapper) : BaseController()
{
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommandDto request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<RegisterCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommandDto request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<LoginCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
