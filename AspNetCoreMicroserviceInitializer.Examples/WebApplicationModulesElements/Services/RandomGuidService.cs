using AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services;

[AutoRegisterService(ServiceLifetime.Transient, typeof(IRandomGuidService))]
public class RandomGuidService : IRandomGuidService
{
    public Guid GetRandomGuid()
    {
        return Guid.NewGuid();
    }
}
