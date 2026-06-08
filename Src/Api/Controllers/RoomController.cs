using Api.DTOs.RoomDtos;
using Application.Requests.RoomRequests;
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
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllRoomsDto request,
        CancellationToken ct)
    {
        var query = mapper.Map<GetAllRooms>(request);
        var result = await mediator.Send(query, ct);
        var resultDto = mapper.Map<Result<PagedResult<RoomDto>>>(result);
        return HandleResult(resultDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new GetRoomById { RoomId = id };
        var result = await mediator.Send(request, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost]
    [Authorize(Roles = $"{UserRoleName.Admin}, {UserRoleName.Manager}")]
    public async Task<IActionResult> InsertAsync([FromBody] InsertRoomDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<InsertRoom>(request);
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{UserRoleName.Admin}, {UserRoleName.Manager}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateRoomDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateRoom>(request);
        command.Id = id;
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{UserRoleName.Admin}, {UserRoleName.Manager}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var request = new DeleteRoom { RoomId = id };
        var result = await mediator.Send(request, ct);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }
}