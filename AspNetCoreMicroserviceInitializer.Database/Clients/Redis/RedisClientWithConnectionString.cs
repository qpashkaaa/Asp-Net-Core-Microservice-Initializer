using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;
using MongoDB.Driver;
using StackExchange.Redis;

namespace AspNetCoreMicroserviceInitializer.Database.Clients.Redis;

/// <summary>
/// Интерфейс клиента Redis, который хранит в себе строку подключения.
/// </summary>
public class RedisClientWithConnectionString : IRedisClientWithConnectionString
{
    /// <summary>
    /// Строка подключения.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Клинет Redis.
    /// </summary>
    private readonly IConnectionMultiplexer _redisClient;

    /// <summary>
    /// Конструктор клиента.
    /// </summary>
    /// <param name="connectionString">Строка подключения</param>
    public RedisClientWithConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        _redisClient = ConnectionMultiplexer.Connect(connectionString);
    }

    /// <summary>
    /// Метод получения клиента Redis.
    /// </summary>
    /// <returns>Клиент Redis типа <see cref="IConnectionMultiplexer"/>.</returns>
    public IConnectionMultiplexer GetClient()
    {
        return _redisClient;
    }

    /// <summary>
    /// Метод получения строки подключения Redis.
    /// </summary>
    /// <returns>Строка подключения Redis.</returns>
    public string GetConnectionString()
    {
        return _connectionString;
    }
}
