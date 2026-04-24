using Application.Hotels.HotelCommands.HotelCommandRequests;
using Application.Hotels.HotelQueries.HotelQueryRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class HotelsController(IMediator mediator) : BaseController()
{
    [HttpGet]
    [Authorize(Roles = "Guest,Admin")]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllHotelsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Guest,Admin")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetHotelByIdQuery() { HotelId = id };
        var result = await mediator.Send(request, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> InsertAsync([FromBody] InsertHotelCommand request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateHotelCommand request,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        var result = await mediator.Send(request, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteHotelCommand
        {
            HotelId = id
        };
        var result = await mediator.Send(request, cancellationToken);
        return HandleResult(result);
    }
}