using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.EnvironmentVariables;

[AutoRegisterConfigSettings]
public class EnvironmentVariablesSettingTest
{
    public string? Message { get; set; }

    public string? OverwriteMessageByEnv { get; set; }
}
