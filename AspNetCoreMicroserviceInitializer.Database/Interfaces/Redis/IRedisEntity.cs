namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;

/// <summary>
/// Интерфейс базовой сущности Redis.
/// </summary>
public interface IRedisEntity
{
    /// <summary>
    /// Ключ.
    /// </summary>
    string Key { get; set; }

    /// <summary>
    /// Значение в формате строки или Json.
    /// </summary>
    string Value { get; set; }
}
