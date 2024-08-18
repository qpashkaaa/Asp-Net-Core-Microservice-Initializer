using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database;

[AutoRegisterRepository]
public class DummyRepository : RepositoryBase<DummyModel>
{
    public DummyRepository(DummyDbContext dbContext) : base(dbContext)
    {
    }
}
