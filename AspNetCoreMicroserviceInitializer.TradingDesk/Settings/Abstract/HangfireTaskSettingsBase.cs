namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

/// <summary>
/// Асбрактный базовый класс настроек для фоновых задач Hangfire.
/// </summary>
public abstract class HangfireTaskSettingsBase
{
    /// <summary>
    /// Cron-выражение периода запуска фоновой задачи.
    ///
    /// Пример: */5 * * * *.
    /// </summary>
    public required string CronExpression { get; init; }

    /// <summary>
    /// Временная зона, в которой указывается время в <see cref="CronExpression"/>.
    ///
    /// Пример: Europe/Moscow.
    /// </summary>
    public required string TimeZone { get; init; }
}