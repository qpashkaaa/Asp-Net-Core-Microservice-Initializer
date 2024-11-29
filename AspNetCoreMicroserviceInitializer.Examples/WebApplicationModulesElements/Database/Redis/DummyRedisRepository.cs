using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;
using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using StackExchange.Redis;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Redis;

[AutoRegisterRepository(interfaceType: typeof(IRedisRepository<DummyRedisEntity>))]
public class DummyRedisRepository : RedisRepositoryBase<DummyRedisEntity>
{
    public DummyRedisRepository(IConnectionMultiplexer multiplexer) : base(multiplexer, "test_repository_redis")
    {
    }
}
