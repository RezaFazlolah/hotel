using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("API/[controller]")]
public class BaseController() : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (!result.IsSuccess)
        {
            if (result.Code == 401)
                return Unauthorized(result.Errors);
            if (result.Code == 404)
                return NotFound(result.Errors);
        }

        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(result.Errors);
    }

    private string ErrorsToString(IEnumerable<Error> errors)
        => string.Join("\n", errors.Select(e => e.Message));
}