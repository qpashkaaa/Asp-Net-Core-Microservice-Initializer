using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Hangfire;

[AutoRegisterService(ServiceLifetime.Transient)]
public class DateTimeService
{
    public async Task<string> GetDateTimeWithMessageAsync()
    {
        var delay = 100;

        await Task.Delay(delay);

        return $"[Delay - {delay}][Current Time] : {DateTime.Now}";
    }
}
