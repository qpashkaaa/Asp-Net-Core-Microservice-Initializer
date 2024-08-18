using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Settings;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly DummySettings _dummySettings;

    public SettingsController(IOptions<DummySettings> dummySettings)
    {
        _dummySettings = dummySettings.Value;
    }

    [HttpGet("GetDummySettingsByAppsettings")]
    public DummySettings GetDummySettingsByAppsettings()
    {
        return _dummySettings;
    }
}
