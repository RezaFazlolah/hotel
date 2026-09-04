using Api.Dtos.RoomDtos;
using Application.Rooms.Commands;
using Application.Rooms.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;

namespace Api.Controllers;

[Authorize]
public class RoomController(
    IMediator mediator,
    IMapper mapper)
    : BaseController
{
    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateRoomCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<CreateRoomCommand>(request);
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var request = new GetRoomByIdQuery(id);
        var result = await mediator.Send(request, ct);
        return HandleResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] GetAllRoomsQueryDto request,
        CancellationToken ct)
    {
        var query = mapper.Map<GetAllRoomsQuery>(request);
        var result = await mediator.Send(query, ct);
        return HandleResult(result);
    }

    [HttpPut("admin/{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
    public async Task<IActionResult> UpdateAsAdminAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateRoomAsAdminCommandBaseDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateRoomAsAdminCommand>(request) with { Id = id };
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpPut("manager/{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Manager)}")]
    public async Task<IActionResult> UpdateAsManagerAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateRoomAsManagerCommandBaseDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateRoomAsManagerCommand>(request) with { Id = id };
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var request = new DeleteRoomCommand(id);
        var result = await mediator.Send(request, ct);
        return HandleResult(result);
    }
}