using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;
using AspNetCoreMicroserviceInitializer.TradingDesk.HangfireFilters;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings;

/// <summary>
/// Модель настроек дашборда Hangfire.
/// </summary>
[AutoRegisterConfigSettings]
[ConfigSettingsModule(WebApplicationModules.Hangfire)]
public class HangfireDashboardSettings
{
    /// <summary>
    /// Включить кастомную авторизацию для дашборда.
    ///
    /// Пример: true/false.
    /// </summary>
    public bool EnableCustomAuthorization { get; init; }

    /// <summary>
    /// Название фильтра авторизации (название класса фильтра).
    ///
    /// Доступные фильтры:
    /// <see cref="AllAuthorizationFilter"/>.
    /// </summary>
    public string? FilterName { get; init; }
}
