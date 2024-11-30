namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

/// <summary>
/// Настройки Redis.
/// </summary>
public abstract class RedisSettingsBase : DbSettingsBase
{
    /// <summary>
    /// Строка подключения.
    /// </summary>
    public override required string ConnectionString { get; init; } = "localhost:6379";
}
