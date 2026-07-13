using Api.Dtos.ReservationDtos;
using Application.Reservations.Commands;
using Application.Reservations.Dtos;
using Application.Reservations.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SharedKernel.Constants;

namespace Api.Controllers;

[Authorize]
public class ReservationController(
    IMediator mediator,
    IMapper mapper)
    : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] GetAllReservationsQueryDto request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetAllReservationsQuery>(request);
        var result = await mediator.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetReservationByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> InsertAsync(
        [FromBody] InsertReservationCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<InsertReservationCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateReservationCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateReservationCommand>(request);
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> CancelAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new CancelReservationCommand(id) { ReservationId = id };
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }
}