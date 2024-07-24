using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.TestApi.Controllers;
[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{

    [HttpGet]
    public void Get()
    {

    }
}
