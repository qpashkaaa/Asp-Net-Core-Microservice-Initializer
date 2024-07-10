namespace AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

/// <summary>
/// Тип хранилища информации о фоновых задачах Hangfire.
/// </summary>
public enum HangfireStorage
{
    /// <summary>
    /// Не определено.
    /// </summary>
    Undefined = 0,
    
    /// <summary>
    /// PostgreSql.
    /// </summary>
    PostgreSql = 1,
    
    /// <summary>
    /// In-Memory.
    /// </summary>
    InMemory = 2
}