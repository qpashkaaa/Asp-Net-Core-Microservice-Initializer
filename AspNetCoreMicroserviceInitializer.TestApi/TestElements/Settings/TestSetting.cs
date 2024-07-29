using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Settings;

[AutoRegisterConfigSettings]
public class TestSetting
{
    public string? Message { get; set; }

    public bool Flag { get; set; }
}
