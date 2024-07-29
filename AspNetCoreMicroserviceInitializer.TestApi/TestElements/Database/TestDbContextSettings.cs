using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Database;

[AutoRegisterConfigSettings]
public class TestDbContextSettings : DbContextSettings
{
}
