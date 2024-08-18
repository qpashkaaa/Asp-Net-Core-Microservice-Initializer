using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Serilog;

[AutoRegisterService(ServiceLifetime.Singleton)]
public class LoggerService
{
    private readonly ILogger<LoggerService> _logger;

    public LoggerService(ILogger<LoggerService> logger)
    {
        _logger = logger;
    }

    public string LogAllStatuses()
    {
        _logger.LogTrace("Trace.");
        _logger.LogDebug("Debug.");
        _logger.LogInformation("Information.");
        _logger.LogWarning("Warning.");
        _logger.LogError("Error.");
        _logger.LogCritical("Critical.");

        return "Sucessfull logged all statuses!";
    }
}
