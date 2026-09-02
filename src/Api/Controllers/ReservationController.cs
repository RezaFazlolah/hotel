using Api.Dtos.ReservationDtos;
using Application.Reservations.Commands;
using Application.Reservations.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
public class ReservationController(
    IMediator mediator,
    IMapper mapper)
    : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] GetAllReservationsQueryDto request,
        CancellationToken ct)
    {
        var query = mapper.Map<GetAllReservationsQuery>(request);
        var result = await mediator.Send(query, ct);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var query = new GetReservationByIdQuery(id);
        var result = await mediator.Send(query, ct);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateReservationCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<CreateReservationCommand>(request);
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpPut("admin/{id:guid}")]
    public async Task<IActionResult> UpdateAsAdminAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateReservationAsAdminCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateReservationAsAdminCommand>(request) with { ReservationId = id };
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpPut("manager/{id:guid}")]
    public async Task<IActionResult> UpdateAsManagerAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateReservationAsManagerCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateReservationAsManagerCommand>(request) with { ReservationId = id };
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpPut("guest/{id:guid}")]
    public async Task<IActionResult> UpdateAsGuestAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateReservationAsGuestCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateReservationAsGuestCommand>(request) with { ReservationId = id };
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> CancelAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var command = new CancelReservationCommand(id) { ReservationId = id };
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }
}