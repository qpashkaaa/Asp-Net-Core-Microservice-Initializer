namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services;

/// <summary>
/// An additional service that is registered through the additional registration method, and not through an attribute.
/// </summary>
public class AdditionalGuidService
{
    /// <summary>
    /// An additional service method.
    /// </summary>
    /// <returns><see cref="Guid"/>.</returns>
    public Guid CreateGuid()
    {
        return Guid.NewGuid();
    }
}
