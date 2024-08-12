namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;

public class ImplementationFactoryService : ServiceWithImplementationFactory
{
    public override string GetMessage()
    {
        return "ImplementationFactoryService (IServiceWithImplementationFactory) message";
    }
}
