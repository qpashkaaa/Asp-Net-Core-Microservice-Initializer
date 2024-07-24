namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.AutoMappers;

public class ModelBase
{
    public string? Message { get; set; }

    public bool Flag { get; set; }

    public int Number { get; set; }

    public string[ ]? Array { get; set; }

    public string? NotMappedProperty { get; set; }
}
