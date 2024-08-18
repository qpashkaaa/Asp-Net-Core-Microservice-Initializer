using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DummyController : ControllerBase
{
    [HttpGet("Ping")]
    public string Ping()
    {
        return "Pong!";
    }
}
