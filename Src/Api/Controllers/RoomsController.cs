using Api.DTOs.RoomDtos;
using Application.Commands.RoomCommands;
using Application.Queries.RoomQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SharedKernel.Constants;

namespace Api.Controllers;

// [Authorize]
public class RoomsController(IMediator mediator, IMapper mapper) : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllRoomsDto request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetAllRooms>(request);
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<ICollection<RoomDto>>>(result);
        return HandleResult(resultDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetRoomById() { RoomId = id };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost]
    // [Authorize(Roles = $"{UserRoleName.Admin},{UserRoleName.Manager}")]
    public async Task<IActionResult> InsertAsync([FromBody] InsertRoomDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<InsertRoom>(request);
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPut("{id:guid}")]
    // [Authorize(Roles = UserRoleName.Admin)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateRoomDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateRoom>(request);
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpDelete("{id:guid}")]
    // [Authorize(Roles = UserRoleName.Admin)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteRoom { RoomId = id };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<RoomDto>>(result);
        return HandleResult(resultDto);
    }
}