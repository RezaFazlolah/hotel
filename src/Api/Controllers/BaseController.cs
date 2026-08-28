using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;
using SharedKernel.Enums;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaseController
    : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.Succeeded)
            return result.Code switch
            {
                ResultCode.Created => Created("", result.Value),
                _ => Ok(result.Value),
            };

        return result.Code switch
        {
            ResultCode.Unauthorized => Unauthorized(ErrorsToString(result.Errors)),
            ResultCode.Forbidden => Forbid(ErrorsToString(result.Errors)),
            ResultCode.NotFound => NotFound(ErrorsToString(result.Errors)),
            _ => BadRequest(ErrorsToString(result.Errors)),
        };
    }

    private static string ErrorsToString(IEnumerable<Error> errors)
        => string.Join("\n", errors.Select(e => e.Message));
}