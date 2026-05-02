using Api.DTOs.AuthDtos;
using Application.Commands.AuthCommands;
using Application.Models;
using AutoMapper;
using Domain.Enums;
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
        command.Role = UserRoles.Guest;
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<UserDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommandDto request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<LoginCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
