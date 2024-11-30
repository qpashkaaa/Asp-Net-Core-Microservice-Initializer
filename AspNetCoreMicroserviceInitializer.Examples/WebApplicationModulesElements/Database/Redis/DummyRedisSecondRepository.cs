using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;
using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.Options;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Redis;

[AutoRegisterRepository]
public class DummyRedisSecondRepository : RedisRepositoryBase<DummyRedisEntity>
{
    public DummyRedisSecondRepository(IRedisClientFactory factory, IOptions<DummyRedisSecondSettings> settings) 
        : base(factory, settings.Value.ConnectionString, "test_repository_redis_second")
    {
    }
}
