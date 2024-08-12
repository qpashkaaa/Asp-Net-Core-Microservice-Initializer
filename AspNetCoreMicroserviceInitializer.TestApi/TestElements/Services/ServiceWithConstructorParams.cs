using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;

[AutoRegisterService(ServiceLifetime.Scoped, typeof(IServiceWithConstructorParams))]
public class ServiceWithConstructorParams : IServiceWithConstructorParams
{
    private readonly ServiceWithoutConstructorParams _serviceWithoutConstructorParams;

    public ServiceWithConstructorParams(ServiceWithoutConstructorParams serviceWithoutConstructorParams)
    {
        _serviceWithoutConstructorParams = serviceWithoutConstructorParams;
    }

    public string MessageFromOtherService()
    {
        return $"ServiceWithConstructorParams (IServiceWithConstructorParams): {_serviceWithoutConstructorParams.MessageFromThisService()}";
    }

    public string MessageFromThisService()
    {
        return $"Message from ServiceWithConstructorParams (IServiceWithConstructorParams)";
    }
}
