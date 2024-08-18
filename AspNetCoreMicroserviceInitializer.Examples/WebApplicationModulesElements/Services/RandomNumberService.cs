using AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services;

[AutoRegisterService(ServiceLifetime.Transient, typeof(IRandomNumberService))]
public class RandomNumberService : IRandomNumberService
{
    public int GetRandomNumber()
    {
        var random = new Random();

        return random.Next();
    }
}
