namespace AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;

/// <summary>
/// Интерфейс фоновой задачи Hangfire.
/// </summary>
public interface IHangfireBackgroundTask
{
    /// <summary>
    /// Асинхронный метод выполнения задачи.
    /// </summary>
    Task ExecuteAsync();
}