using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Settings;

[AutoRegisterConfigSettings]
public class DummySettings
{
    public required string DummyText { get; set; }

    public bool DummyFlag { get; set; }

    public double DummyNumber { get; set; }

    public string[ ]? DummyArray { get; set; }
}
