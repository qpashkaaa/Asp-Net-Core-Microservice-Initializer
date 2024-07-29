using AspNetCoreMicroserviceInitializer.Database.Interfaces;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Database;

public class TestTable : IEntity<long>
{
    public long Id { get; set; }

    public string Message { get; set; }

    public DateTime CreationDate { get; set; }
}
