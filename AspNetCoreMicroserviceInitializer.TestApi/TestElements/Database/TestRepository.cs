using AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Database;

[AutoRegisterRepository]
public class TestRepository : RepositoryBase<TestTable>
{
    public TestRepository(TestDbContext dbContext) : base(dbContext)
    {
    }
}
