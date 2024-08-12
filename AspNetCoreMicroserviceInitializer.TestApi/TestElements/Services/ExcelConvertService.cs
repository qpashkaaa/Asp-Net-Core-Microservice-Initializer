using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;

[AutoRegisterService(ServiceLifetime.Transient)]
public class ExcelConvertService
{
    public string Convert()
    {
        return "ExcelConverted";
    }
}
