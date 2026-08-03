using Microsoft.AspNetCore.Mvc;
using SharedKernel.Common;

namespace Api.Controllers;

public class TestController
    : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Test()
    {
        var error1= new Error("error1");
        var error2 = new Error("error2", innerError: error1);
        var error3= new Error("error3", innerError: error2);
        
        Console.WriteLine(error1);
        Console.WriteLine(error2);
        Console.WriteLine(error3);
        _ = 3 + 5;
        throw new NotImplementedException();
    }
}