using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Hangfire;

[AutoRegisterHangfireTask(typeof(DateTimeTaskSettings))]
public class DateTimeTask : IHangfireBackgroundTask
{
    private readonly DateTimeService _dateTimeService;
    private readonly ILogger<DateTimeTask> _logger;

    public DateTimeTask(
        DateTimeService dateTimeService,
        ILogger<DateTimeTask> logger)
    {
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var currentDateTime = await _dateTimeService.GetDateTimeWithMessageAsync();

        _logger.LogInformation(currentDateTime);
    }
}
