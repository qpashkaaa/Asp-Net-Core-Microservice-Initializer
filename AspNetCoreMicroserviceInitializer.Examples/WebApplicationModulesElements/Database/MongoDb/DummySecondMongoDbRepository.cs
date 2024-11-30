using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.Options;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.MongoDb;

[AutoRegisterRepository]
public class DummySecondMongoDbRepository : MongoRepositoryBase<DummyMongoDbEntity>
{
    public DummySecondMongoDbRepository(IMongoClientFactory factory, IOptions<DummySecondMongoSettings> settings)
        : base(factory, settings.Value.ConnectionString, settings.Value.DatabaseName)
    {
    }
}
