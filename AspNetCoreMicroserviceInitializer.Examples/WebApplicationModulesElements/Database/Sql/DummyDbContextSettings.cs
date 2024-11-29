using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Sql;

[AutoRegisterConfigSettings]
public class DummyDbContextSettings : DbContextSettings
{
}
