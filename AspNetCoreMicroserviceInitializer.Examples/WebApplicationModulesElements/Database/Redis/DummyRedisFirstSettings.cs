using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Redis;

[AutoRegisterConfigSettings]
public class DummyRedisFirstSettings : RedisSettingsBase
{
}
