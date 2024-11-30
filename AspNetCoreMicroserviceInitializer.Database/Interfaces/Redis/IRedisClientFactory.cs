using StackExchange.Redis;

namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;

/// <summary>
/// Интерфейс фабрики для получения клиента Redis.
/// </summary>
/// <remarks>
/// Данный сервис уже зарегистрирован в DI, если вы подключили модуль WebApplicationModules.RedisDatabase.
/// Его нужно просто добавить в конструктор репозитория-наследника.
/// </remarks>
public interface IRedisClientFactory : IClientFactory<IConnectionMultiplexer>
{
}
