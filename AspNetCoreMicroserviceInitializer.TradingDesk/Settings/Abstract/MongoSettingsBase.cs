namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

/// <summary>
/// Настройки MongoDb.
/// </summary>
public abstract class MongoSettingsBase : DbSettingsBase
{
    /// <summary>
    /// Строка подключения.
    /// </summary>
    public override required string ConnectionString { get; init; } = "mongodb://localhost:27017";

    /// <summary>
    /// Имя базы данных MongoDb.
    /// </summary>
    public required string DatabaseName { get; init; } = "database";
}
