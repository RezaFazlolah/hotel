using Api.DTOs.ReservationDtos;
using Application.Commands.ReservationCommands;
using Application.Models;
using Application.Queries.ReservationQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

// [Authorize]
public class ReservationsController(IMediator mediator, IMapper mapper) : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllByGuestAsync(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guestId))
            return Unauthorized();

        var request = new GetAllReservationsQuery() { GuestId = guestId };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<ICollection<ReservationDto>>>(result);
        return HandleResult(resultDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guestId))
            return Unauthorized();

        var request = new GetReservationByIdQuery() { GuestId = guestId, ReservationId = id };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<ReservationDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> InsertByGuestAsync([FromBody] InsertReservationCommandDto request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guestId))
            return Unauthorized();

        var insertReservationCommand = mapper.Map<InsertReservationCommand>(request);
        insertReservationCommand.GuestId = guestId;

        var result = await mediator.Send(insertReservationCommand, cancellationToken);
        var resultDto = mapper.Map<Result<ReservationDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPut("{id:guid}")]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateByGuestAsync(Guid id, [FromBody] UpdateReservationCommandDto request,
        CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guestId))
            return Unauthorized();

        request.ReservationId = id;
        var request2 = mapper.Map<UpdateReservationCommand>(request);
        request2.GuestId = guestId;
        var result = await mediator.Send(request2, cancellationToken);
        var resultDto = mapper.Map<Result<ReservationDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpDelete("{id:guid}")]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteByGuestAsync(Guid id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var guestId))
            return Unauthorized();

        var request = new DeleteReservationCommand() { ReservationId = id, GuestId = guestId };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<ReservationDto>>(result);
        return HandleResult(resultDto);
    }
}