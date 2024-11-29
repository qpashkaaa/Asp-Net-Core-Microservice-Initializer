using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using MongoDB.Driver;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.MongoDb;

[AutoRegisterRepository(interfaceType: typeof(IMongoDbRepository<DummyMongoDbEntity>))]
public class DummyMongoDbRepository : MongoDbRepositoryBase<DummyMongoDbEntity>
{
    public DummyMongoDbRepository(IMongoDatabase database) : base(database)
    {
    }
}
