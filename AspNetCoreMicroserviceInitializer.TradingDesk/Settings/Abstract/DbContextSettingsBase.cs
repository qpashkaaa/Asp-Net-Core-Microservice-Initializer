﻿namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

/// <summary>
/// Настройки контекста БД.
/// </summary>
public abstract class DbContextSettingsBase : DbSettingsBase
{
    /// <summary>
    /// Строка подключения.
    /// 
    /// Пример: Host=localhost:5555; Database=postgres; Username=postgres; Password=postgres
    /// </summary>
    public override required string ConnectionString { get; init; } = "Host=localhost:5555; Database=postgres; Username=postgres; Password=postgres";

    /// <summary>
    /// Наименование схемы.
    ///
    /// Пример: MySchema.
    /// </summary>
    public required string Schema { get; init; } = "MySchema";

    /// <summary>
    /// Наименование таблицы миграций.
    ///
    /// Пример: __EFMigrationsHistory.
    /// </summary>
    public required string MigrationsTableName { get; init; } = "__EFMigrationsHistory";

    /// <summary>
    /// Схема для таблицы миграций.
    ///
    /// Пример: MySchema.
    /// </summary>
    public required string MigrationsSchema { get; init; } = "MySchema";
}
