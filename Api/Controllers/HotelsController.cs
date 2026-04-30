using Api.DTOs.HotelDtos;
using Application.Commands.HotelCommands;
using Application.Models;
using Application.Queries.HotelQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class HotelsController(IMediator mediator, IMapper mapper) : BaseController()
{
    [HttpGet]
    // [Authorize(Roles = "Guest,Admin")]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllHotelsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<ICollection<HotelDto>>>(result);
        return HandleResult(resultDto);
    }

    [HttpGet("{id:guid}")]
    // [Authorize(Roles = "Guest,Admin")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetHotelByIdQuery() { HotelId = id };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> InsertAsync([FromBody] InsertHotelCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<InsertHotelCommand>(request);
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPut("{id:guid}")]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateHotelCommandDto request,
        CancellationToken cancellationToken)
    {
        var command = mapper.Map<UpdateHotelCommand>(request);
        command.Id = id;
        var result = await mediator.Send(command, cancellationToken);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpDelete("{id:guid}")]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteHotelCommand
        {
            HotelId = id
        };
        var result = await mediator.Send(request, cancellationToken);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }
}