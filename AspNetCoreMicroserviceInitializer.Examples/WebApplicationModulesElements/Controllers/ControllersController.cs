using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ControllersController : ControllerBase
{
    [HttpGet("Ping")]
    public string Ping()
    {
        return "Pong!";
    }
}
