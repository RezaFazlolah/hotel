using Api.DTOs.HotelDtos;
using Application.Requests.HotelRequests;
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
public class HotelController(IMediator mediator, IMapper mapper) : BaseController()
{
    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] GetAllHotelsDto request, CancellationToken ct)
    {
        var query = mapper.Map<GetAllHotels>(request);
        var result = await mediator.Send(query, ct);
        var resultDto = mapper.Map<Result<PagedResult<HotelDto>>>(result);
        return HandleResult(resultDto);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var query = new GetHotelById { HotelId = id };
        var result = await mediator.Send(query, ct);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPost]
    [Authorize(Roles = UserRoleName.Admin)]
    public async Task<IActionResult> InsertAsync([FromBody] InsertHotelDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<InsertHotel>(request);
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpPut("{id:guid}")]
    // [Authorize(Roles = UserRoleName.Admin)]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateHotelDto request,
        CancellationToken ct)
    {
        var command = mapper.Map<UpdateHotel>(request);
        command.Id = id;
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{UserRoleName.Admin}, {UserRoleName.Manager}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, CancellationToken ct)
    {
        var command = new DeleteHotel { HotelId = id };
        var result = await mediator.Send(command, ct);
        var resultDto = mapper.Map<Result<HotelDto>>(result);
        return HandleResult(resultDto);
    }
}