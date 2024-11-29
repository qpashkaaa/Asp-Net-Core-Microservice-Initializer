using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Redis;

public class DummyRedisEntity : IRedisEntity
{
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
