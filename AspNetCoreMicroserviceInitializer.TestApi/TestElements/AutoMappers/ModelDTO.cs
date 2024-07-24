namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.AutoMappers;

public record ModelDTO
{
    public string? Message { get; init; }

    public bool Flag { get; init; }

    public int Number { get; init; }

    public string[ ]? Array { get; init; }
}
