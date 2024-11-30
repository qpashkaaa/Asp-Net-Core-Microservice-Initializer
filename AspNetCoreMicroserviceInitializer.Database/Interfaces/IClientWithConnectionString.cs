namespace AspNetCoreMicroserviceInitializer.Database.Interfaces;

/// <summary>
/// Интерфейс клиента со строкой подключения.
/// </summary>
public interface IClientWithConnectionString<TClient>
{
    /// <summary>
    /// Метод получения клиента Redis.
    /// </summary>
    /// <returns>Клиент Redis.</returns>
    TClient GetClient();

    /// <summary>
    /// Метод получения строки подключения.
    /// </summary>
    /// <returns>Строка подключения.</returns>
    string GetConnectionString();
}
