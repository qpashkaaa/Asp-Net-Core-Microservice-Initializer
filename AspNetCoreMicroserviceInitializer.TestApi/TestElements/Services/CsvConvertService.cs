using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;

[AutoRegisterService(ServiceLifetime.Transient)]
public class CsvConvertService
{
    public string Convert()
    {
        return "CsvConverted";
    }
}
