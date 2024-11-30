namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

/// <summary>
/// Базовый класс настроек базы данных.
/// </summary>
public abstract class DbSettingsBase
{
    /// <summary>
    /// Строка подключения.
    /// </summary>
    public virtual required string ConnectionString { get; init; } = string.Empty;
}
