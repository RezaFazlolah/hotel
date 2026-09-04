using Api.Dtos.HotelDtos;
using Application.Hotels.Commands;
using Application.Hotels.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Enums;

namespace Api.Controllers;

[Authorize]
public class HotelController(
    IMediator mediator,
    IMapper mapper)
    : BaseController
{
    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateHotelCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<CreateHotelCommand>(request);
        var result = await mediator.Send(command, ct);
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

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] GetAllHotelsQueryDto request,
        CancellationToken ct)
    {
        var query = mapper.Map<GetAllHotelsQuery>(request);
        var result = await mediator.Send(query, ct);
        return HandleResult(result);
    }

    [HttpPut("admin/{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
    public async Task<IActionResult> UpdateAsAdminAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateHotelAsAdminCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateHotelAsAdminCommand>(request) with { Id = id };
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpPut("manager/{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Manager)}")]
    public async Task<IActionResult> UpdateAsManagerAsync(
        [FromRoute] Guid id,
        [FromBody] UpdateHotelAsManagerCommandDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateHotelAsManagerCommand>(request) with { Id = id };
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}, {nameof(UserRole.Manager)}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid id,
        CancellationToken ct)
    {
        var command = new DeleteHotelCommand(id);
        var result = await mediator.Send(command, ct);
        return HandleResult(result);
    }
}