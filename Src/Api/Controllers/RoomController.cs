using Api.DTOs.RoomDtos;
using Application.Rooms.Commands;
using Application.Rooms.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SharedKernel.Constants;
using SharedKernel.Paging;

namespace Api.Controllers;

[Authorize]
public class RoomController(IMediator mediator, IMapper mapper) : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllRoomsQueryDto request,
        CancellationToken ct)
    {
        var query = mapper.Map<GetAllRoomsQuery>(request);
        var result = await mediator.Send(query, ct);
        var resultDto = mapper.Map<Result<PagedResult<RoomDto>>>(result);
        return HandleResult(resultDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new GetRoomByIdQuery { RoomId = id };
        var result = await mediator.Send(request, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoleName.Admin}, {UserRoleName.Manager}")]
    public async Task<IActionResult> InsertAsync([FromBody] InsertRoomCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<InsertRoomCommand>(request);
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{UserRoleName.Admin}, {UserRoleName.Manager}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateRoomCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateRoomCommand>(request);
        command.Id = id;
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{UserRoleName.Admin}, {UserRoleName.Manager}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new DeleteRoomCommand { RoomId = id };
        var result = await mediator.Send(request, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }
}