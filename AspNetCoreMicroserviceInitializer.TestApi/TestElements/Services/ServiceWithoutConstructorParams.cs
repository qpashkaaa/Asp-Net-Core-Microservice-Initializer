using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;

[AutoRegisterService(ServiceLifetime.Transient)]
public class ServiceWithoutConstructorParams
{
    public string MessageFromThisService()
    {
        return "Hello from ServiceWithoutConstructorParams";
    }
}
