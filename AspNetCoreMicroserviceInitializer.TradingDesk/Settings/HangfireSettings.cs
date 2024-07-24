using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings;

/// <summary>
/// Модель настроек Hangfire.
/// </summary>
[AutoRegisterConfigSettings]
[ConfigSettingsModule(WebApplicationModules.Hangfire)]
public class HangfireSettings
{
    /// <summary>
    /// Тип хранилища информации о фоновых задачах Hangfire.
    ///
    /// Пример: 1.
    /// </summary>
    public HangfireStorage StorageType { get; init; } = HangfireStorage.InMemory;
    
    /// <summary>
    /// Строка подключения к PostgreSql для хранения данных о фоновых задачах Hangfire (заполняется, если <see cref="StorageType"/> выбран PostgreSql).
    ///
    /// Пример: Host=localhost:5555; Database=postgres; Username=postgres; Password=postgres.
    /// </summary>
    public string? PostgreSqlConnectionString { get; init; }
}