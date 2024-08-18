using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Serilog;

[Route("api/[controller]")]
[ApiController]
public class SerilogController : ControllerBase
{
    private readonly ILogger<SerilogController> _logger;
    private readonly LoggerService _loggerService;

    public SerilogController(
        ILogger<SerilogController> logger,
        LoggerService loggerService)
    {
        _logger = logger;
        _loggerService = loggerService;
    }

    [HttpGet("Log")]
    public string Log()
    {
        _logger.LogInformation($"[{nameof(SerilogController)}] : Logging all statuses from controller!");

        return _loggerService.LogAllStatuses();
    }
}
