using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;
using MongoDB.Driver;
using StackExchange.Redis;

namespace AspNetCoreMicroserviceInitializer.Database.Services.Redis;

/// <summary>
/// Фабрика для получения клиента Redis.
/// </summary>
public class RedisClientFactory : IRedisClientFactory
{
    /// <summary>
    /// Клиенты Redis.
    /// </summary>
    private readonly IEnumerable<IRedisClientWithConnectionString> _clients;

    /// <summary>
    /// Кнструктор фабрики.
    /// </summary>
    /// <param name="clients">Клиенты Redis.</param>
    public RedisClientFactory(
        IEnumerable<IRedisClientWithConnectionString> clients)
    {
        _clients = clients;
    }

    /// <summary>
    /// Метод получения клиента по строке подключения.
    /// </summary>
    /// <param name="connectionString">Строка подключения.</param>
    /// <returns>Клиент типа <see cref="IConnectionMultiplexer"/>.</returns>
    public IConnectionMultiplexer GetClientByConnectionString(string connectionString)
    {
        return _clients
            .Where(client => client.GetConnectionString() == connectionString)
            .First()
            .GetClient();
    }
}
