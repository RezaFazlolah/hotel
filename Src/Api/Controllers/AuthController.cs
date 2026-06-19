using Api.DTOs.AuthDtos;
using Application.Auth.Commands;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SharedKernel.Constants;
using SharedKernel.Enums;

namespace Api.Controllers;

public class AuthController(IMediator mediator, IMapper mapper) : BaseController()
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<RegisterCommand>(request) with {Role = UserRole.Guest};
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<UserDto>>(result);
        return HandleResult(resultDto);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommandDto request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<LoginCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [Authorize(Roles = UserRoleName.Admin)]
    [HttpPost("registerByAdmin")]
    public async Task<IActionResult> RegisterByAdminAsyc([FromBody] RegisterByAdminCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<RegisterCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<UserDto>>(result);
        return HandleResult(resultDto);
    }
}