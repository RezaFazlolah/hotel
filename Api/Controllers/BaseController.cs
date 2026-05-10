using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController() : ControllerBase
{
    // protected Guid? RequestId
    // {
    //     get
    //     {
    //         var userIdAsString = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
    //         return Guid.TryParse(userIdAsString, out var userId)
    //             ? userId
    //             : null;
    //     }
    // }

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            if (result.Code == 201)
                return Created("", result.Value);
            return Ok(result.Value);
        }
        else
        {
            if (result.Code == 401)
                return Unauthorized(result.Errors);
            if (result.Code == 404)
                return NotFound(result.Errors);
            return BadRequest(result.Errors);
        }
    }

    private string ErrorsToString(IEnumerable<Error> errors)
        => string.Join("\n", errors.Select(e => e.Message));
}