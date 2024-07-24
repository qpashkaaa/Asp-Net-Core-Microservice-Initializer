using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Hangfire;

[AutoRegisterConfigSettings]
public class TestTaskSettings : HangfireTaskSettingsBase
{
}
