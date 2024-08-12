using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;

[AutoRegisterService(ServiceLifetime.Transient, typeof(IServiceWithImplementationFactory))]
public class ServiceWithImplementationFactory : IServiceImplementationFactory<ServiceWithImplementationFactory>, IServiceWithImplementationFactory
{
    public virtual string GetMessage()
    {
        return "Message from ServiceWithImplementationFactory";
    }

    public Func<IServiceProvider, ServiceWithImplementationFactory> ImplementationFactory => sp => new ImplementationFactoryService();
}
