using AspNetCoreMicroserviceInitializer.Database.Interfaces;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database;

public class DummyModel : IEntity<long>
{
    public long Id { get; set; }

    public string? Message { get; set; }

    public int Number { get; set; }

    public bool Status { get; set; }

    public List<string>? AdditionalMessages { get; set; }

    public DateTime CreationDate { get; set; }
}
