using Api.Dtos.RoomDtos;
using Application.Rooms.Commands;
using Application.Rooms.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Constants;

namespace Api.Controllers;

[Authorize]
public class RoomController(
    IMediator mediator,
    IMapper mapper)
    : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] GetAllRoomsQueryDto request,
        CancellationToken ct)
    {
        var query = mapper.Map<GetAllRoomsQuery>(request);
        var result = await mediator.Send(query, ct);
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

    [HttpPost]
    [Authorize(Roles = $"{UserRoleAsString.Admin}, {UserRoleAsString.Manager}")]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateRoomCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<CreateRoomCommand>(request);
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{UserRoleAsString.Admin}, {UserRoleAsString.Manager}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateRoomCommandDto request,
        CancellationToken ct)
    {
        // future: i get error, its an EF Core tracking problem, fix it later when you read EF Core in details
        var command = mapper.Map<UpdateRoomCommand>(request) with {Id = id};
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{UserRoleAsString.Admin}, {UserRoleAsString.Manager}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var request = new DeleteRoomCommand(id);
        var result = await mediator.Send(request, ct);
        return HandleResult(result);
    }
}