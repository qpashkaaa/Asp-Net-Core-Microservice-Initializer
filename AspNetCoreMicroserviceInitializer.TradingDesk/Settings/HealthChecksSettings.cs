using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings;

/// <summary>
/// Модель настроек Health Checks из конфига appsettings.
/// </summary>
[AutoRegisterConfigSettings]
[ConfigSettingsModule(WebApplicationModules.HealthChecks)]
public class HealthChecksSettings
{
    /// <summary>
    /// Endpoint Health Checks.
    /// 
    /// Пример: /health.
    /// </summary>
    public required string Endpoint { get; init; } = "/health";

    /// <summary>
    /// Включить UI для отслеживание Health Checks.
    /// 
    /// Пример: true/false.
    /// </summary>
    public bool UIEnable { get; init; } = true;

    /// <summary>
    /// Полный Url Health Check endpoint'а для запросов из Health Checks UI.
    /// 
    /// Пример: http://mydomain:80/health.
    /// </summary>
    public string? EndpointFullUrl { get; init; } = "http://localhost:8000/health";

    /// <summary>
    /// Частота опроса в секундах Health Checks для UI.
    /// 
    /// Пример: 10.
    /// </summary>
    public int? UIEvaluationTimeInSeconds { get; init; } = 5;

    /// <summary>
    /// Максимальное кол-во активных (одновременных) запросов на UI Health Checks Api.
    /// 
    /// Пример: 2.
    /// </summary>
    public int? UIApiMaxActiveRequests { get; init; } = 2;
}
