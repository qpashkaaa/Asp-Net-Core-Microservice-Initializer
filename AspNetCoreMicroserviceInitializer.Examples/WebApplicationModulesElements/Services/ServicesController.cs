using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly RandomService _randomService;
    private readonly AdditionalGuidService _additionalGuidService;

    public ServicesController(
        RandomService randomService,
        AdditionalGuidService additionalGuidService)
    {
        _randomService = randomService;
        _additionalGuidService = additionalGuidService;
    }

    [HttpGet("GetRandomValues")]
    public Dictionary<string, object> GetRandomValues()
    {
        return _randomService.GetRandomValues();
    }

    [HttpGet("GetRandomGuid")]
    public Guid GetRandomGuid()
    {
        return _additionalGuidService.CreateGuid();
    }
}
