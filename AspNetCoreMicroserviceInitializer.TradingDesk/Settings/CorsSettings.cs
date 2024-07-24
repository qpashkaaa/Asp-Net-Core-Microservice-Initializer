using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings;

/// <summary>
/// Настройки Cors.
/// </summary>
[AutoRegisterConfigSettings]
[ConfigSettingsModule(WebApplicationModules.Cors)]
public class CorsSettings
{
    /// <summary>
    /// Флаг включения Cors с именованной политикой.
    ///
    /// Пример: true/false.
    /// </summary>
    public bool EnableCors { get; init; } = true;

    /// <summary>
    /// Наименование политики Cors.
    ///
    /// Пример: AllowAccessFrontendQueries.
    /// </summary>
    public string? PolicyName { get; init; } = "AllowAccessFrontendQueries";

    /// <summary>
    /// Список доменов для политики Cors.
    ///
    /// Пример: http://localhost:8082.
    /// </summary>
    public string[ ]? AllowedOrigins { get; init; }
}
