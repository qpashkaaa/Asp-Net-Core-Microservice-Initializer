namespace AspNetCoreMicroserviceInitializer.Database.Interfaces;

/// <summary>
/// Базовый интерфейс фабрики клиентов.
/// </summary>
/// <typeparam name="TClient">Тип клиента.</typeparam>
public interface IClientFactory<TClient>
{
    /// <summary>
    /// Метод получения клиента по строке подключения.
    /// </summary>
    /// <param name="connectionString">Строка подключения.</param>
    /// <returns>Клиент типа <see cref="TClient"/>.</returns>
    TClient GetClientByConnectionString(string connectionString);
}
