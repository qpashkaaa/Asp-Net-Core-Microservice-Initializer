using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings;

/// <summary>
/// Настройки мигратора.
/// </summary>
[AutoRegisterConfigSettings]
[ConfigSettingsModule(WebApplicationModules.EFMigrations)]
public class MigratorSettings
{
    /// <summary>
    /// Флаг, отвечающий за то, нужно ли выключить приложение после выполнения миграций.
    /// </summary>
    public bool IsStopApplicationAfterApplyMigrations { get; init; } = false;
}
