using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class TestController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Test()
    {

        throw new NotImplementedException();
    }
}