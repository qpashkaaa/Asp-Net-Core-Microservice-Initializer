namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

/// <summary>
/// Настройки контекста БД PostgreSql.
/// </summary>
public abstract class DbContextSettings
{
    /// <summary>
    /// Строка подключения.
    ///
    /// Пример: Host=localhost:5555; Database=postgres; Username=postgres; Password=postgres
    /// </summary>
    public required string ConnectionString { get; init; }
    
    /// <summary>
    /// Наименование схемы.
    ///
    /// Пример: MySchema.
    /// </summary>
    public required string Schema { get; init; }

    /// <summary>
    /// Наименование таблицы миграций.
    ///
    /// Пример: __EFMigrationsHistory.
    /// </summary>
    public required string MigrationsTableName { get; init; }

    /// <summary>
    /// Схема для таблицы миграций.
    ///
    /// Пример: MySchema.
    /// </summary>
    public required string MigrationsSchema { get; init; }
}
