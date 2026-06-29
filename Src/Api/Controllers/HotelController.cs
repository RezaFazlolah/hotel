using Api.Dtos.HotelDtos;
using Application.Hotels.Commands;
using Application.Hotels.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using SharedKernel.Common;
using SharedKernel.Constants;
using SharedKernel.Paging;

namespace Api.Controllers;

[Authorize]
public class HotelController(IMediator mediator, IMapper mapper)
    : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] GetAllHotelsQueryDto request,
        CancellationToken ct)
    {
        var query = mapper.Map<GetAllHotelsQuery>(request);
        var result = await mediator.Send(query, ct);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var query = new GetHotelByIdQuery(id);
        var result = await mediator.Send(query, ct);
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = UserRoleAsString.Admin)]
    public async Task<IActionResult> InsertAsync(
        [FromBody] InsertHotelCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<InsertHotelCommand>(request);
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{UserRoleAsString.Admin}, {UserRoleAsString.Manager}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateHotelCommandDto request,
        CancellationToken ct)
    {
        var command = new UpdateHotelCommand(id, request.Name, request.Address, request.Rating);
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{UserRoleAsString.Admin}, {UserRoleAsString.Manager}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var command = new DeleteHotelCommand(id);
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }
}