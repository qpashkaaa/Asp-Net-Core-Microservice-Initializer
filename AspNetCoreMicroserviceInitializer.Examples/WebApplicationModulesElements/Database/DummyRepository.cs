using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database;

[AutoRegisterRepository]
public class DummyRepository : SqlRepositoryBase<DummyModel>
{
    public DummyRepository(DummyDbContext dbContext) : base(dbContext)
    {
    }
}
