using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class DevController : BaseController
{
    [HttpGet("guid/{number:int}")]
    public async Task<IActionResult> GenerateGuidAsync([FromQuery] int count, CancellationToken ct)
    {
        var result = new List<Guid>();
        for (var i = 0; i < count; i++)
            result.Add(Guid.NewGuid());

        return Ok(result);
    }
}