using AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;
using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.Sql;

[AutoRegisterRepository(interfaceType: typeof(ISqlRepository<DummyModel>))]
public class DummyRepository : SqlRepositoryBase<DummyModel>
{
    public DummyRepository(DummyDbContext dbContext) : base(dbContext)
    {
    }
}
