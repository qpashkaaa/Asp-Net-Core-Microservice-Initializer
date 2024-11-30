using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;
using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.Options;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Redis;

[AutoRegisterRepository(interfaceType: typeof(IRedisRepository<DummyRedisEntity>))]
public class DummyRedisFirstRepository : RedisRepositoryBase<DummyRedisEntity>
{
    public DummyRedisFirstRepository(IRedisClientFactory factory, IOptions<DummyRedisFirstSettings> settings) 
        : base(factory, settings.Value.ConnectionString, "test_repository_redis_first")
    {
    }
}
