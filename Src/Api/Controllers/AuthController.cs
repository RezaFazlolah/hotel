using Api.DTOs.AuthDtos;
using Application.Requests.AuthRequests;
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
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<Register>(request);
        command.Role = UserRole.Guest;
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<UserDto>>(result);
        return HandleResult(resultDto);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginDto request, CancellationToken cancellationToken)
    {
        var command = mapper.Map<Login>(request);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [Authorize(Roles = UserRoleName.Admin)]
    [HttpPost("registerByAdmin")]
    public async Task<IActionResult> RegisterByAdminAsyc([FromBody] RegisterByAdminDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<Register>(request);
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<UserDto>>(result);
        return HandleResult(resultDto);
    }
}