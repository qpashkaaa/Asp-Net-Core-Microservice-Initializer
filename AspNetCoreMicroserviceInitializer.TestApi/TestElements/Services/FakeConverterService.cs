using System.Text;
using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;

[AutoRegisterService(ServiceLifetime.Singleton, typeof(IFakeConverterService))]
public class FakeConverterService : IFakeConverterService
{
    private readonly CsvConvertService _csvConvetService;
    private readonly ExcelConvertService _excelConvertService;

    public FakeConverterService(
        CsvConvertService csvConvetService,
        ExcelConvertService excelConvertService)
    {
        _csvConvetService = csvConvetService;
        _excelConvertService = excelConvertService;
    }

    public virtual string Convert()
    {
        var sb = new StringBuilder();
        sb.AppendLine(_csvConvetService.Convert());
        sb.AppendLine(_excelConvertService.Convert());
        return sb.ToString();
    }
}
