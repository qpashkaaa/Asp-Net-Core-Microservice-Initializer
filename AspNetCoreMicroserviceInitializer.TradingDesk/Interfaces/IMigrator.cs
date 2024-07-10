namespace AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;

/// <summary>
/// Интерфейс мигратора.
/// </summary>
public interface IMigrator
{
    /// <summary>
    /// Метод выполнения миграции.
    /// </summary>
    Task MigrateAsync(CancellationToken cancellationToken);
}