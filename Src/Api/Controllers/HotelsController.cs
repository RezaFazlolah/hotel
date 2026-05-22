using Api.DTOs.HotelDtos;
using Application.Commands.HotelCommands;
using Application.Queries.HotelQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using SharedKernel.Common;
using SharedKernel.Constants;

namespace Api.Controllers;

[Authorize]
public class HotelsController(IMediator mediator, IMapper mapper) : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllHotelsQuery request,
        CancellationToken ct)
    {
        var result = await mediator.Send(request, ct);
        var resultDto = mapper.Map<Result<ICollection<HotelDto>>>(result);
        return HandleResult(resultDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetHotelByIdQuery { HotelId = id };
        var result = await mediator.Send(query, ct);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost]
    [Authorize(Roles = UserRoleNames.Admin)]
    public async Task<IActionResult> InsertAsync([FromBody] InsertHotelCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<InsertHotelCommand>(request);
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPut("{id:guid}")]
    // [Authorize(Roles = UserRoleNames.Admin)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateHotelCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateHotelCommand>(request);
        command.Id = id;
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{UserRoleNames.Admin}, {UserRoleNames.Manager}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new DeleteHotelCommand { HotelId = id };
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }
}