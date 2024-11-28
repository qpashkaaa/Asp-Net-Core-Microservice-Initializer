using AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database;

public class DummyModel : ISqlEntity<long>
{
    public long Id { get; set; }

    public string? Message { get; set; }

    public int Number { get; set; }

    public bool Status { get; set; }

    public List<string>? AdditionalMessages { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public bool IsDeleted { get; set; }
}
