using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class DevController
    : BaseController
{
    [HttpPost("guid")]
    public async Task<IActionResult> GenerateGuidAsync([FromBody] int count = 1)
    {
        var result = new List<Guid>();
        for (var i = 0; i < count; i++)
            result.Add(Guid.NewGuid());

        return Ok(result);
    }
}