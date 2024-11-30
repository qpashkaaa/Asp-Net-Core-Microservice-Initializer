using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.Options;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.MongoDb;

[AutoRegisterRepository(interfaceType: typeof(IMongoRepository<DummyMongoDbEntity>))]
public class DummyFirstMongoDbRepository : MongoRepositoryBase<DummyMongoDbEntity>
{
    public DummyFirstMongoDbRepository(IMongoClientFactory factory, IOptions<DummyFirstMongoSettings> settings) 
        : base(factory, settings.Value.ConnectionString, settings.Value.DatabaseName)
    {
    }
}
