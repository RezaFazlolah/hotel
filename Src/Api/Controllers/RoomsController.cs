using Api.DTOs.RoomDtos;
using Application.Commands.RoomCommands;
using Application.Models;
using Application.Queries.RoomQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

// [Authorize]
public class RoomsController(IMediator mediator, IMapper mapper) : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllRoomsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<ICollection<RoomDto>>>(result);
        return HandleResult(resultDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetRoomByIdQuery() { RoomId = id };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost]
    // [Authorize(Roles = UserRoleNames.Admin)]
    public async Task<IActionResult> InsertAsync([FromBody] InsertRoomCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<InsertRoomCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPut("{id:guid}")]
    // [Authorize(Roles = UserRoleNames.Admin)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateRoomCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateRoomCommand>(request);
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpDelete("{id:guid}")]
    // [Authorize(Roles = UserRoleNames.Admin)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteRoomCommand { RoomId = id };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }
}