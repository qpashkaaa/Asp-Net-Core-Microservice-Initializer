using StackExchange.Redis;

namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;

/// <summary>
/// Интерфейс клиента Redis со строкой подключения.
/// </summary>
public interface IRedisClientWithConnectionString : IClientWithConnectionString<IConnectionMultiplexer>
{
}
