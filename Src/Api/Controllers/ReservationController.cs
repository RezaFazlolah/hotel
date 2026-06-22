using Api.Dtos.ReservationDtos;
using Application.Dtos.ReservationDtos;
using Application.Reservations.Commands;
using Application.Reservations.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SharedKernel.Constants;

namespace Api.Controllers;

// [Authorize]
public class ReservationController(IMediator mediator, IMapper mapper)
    : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllReservationsQueryDto request,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetAllReservationsQuery>(request);
        var result = await mediator.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        // var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        // if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guestId))
        //     return Unauthorized();
        //
        // var request = new GetReservationByIdQuery() { GuestId = guestId, ReservationId = id };
        // var result = await mediator.Send(request, cancellationToken);
        // return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = UserRoleName.Guest)]
    public async Task<IActionResult> InsertAsync([FromBody] InsertReservationCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<InsertReservationCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = UserRoleName.Guest)]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateReservationCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateReservationCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = UserRoleName.Admin)]
    public async Task<IActionResult> CancelAsync(Guid id, CancellationToken cancellationToken)
    {
        var command = new CancelReservationCommand(id) { ReservationId = id };
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<ReservationDto>>(result);
        return HandleResult(resultDto);
    }
}