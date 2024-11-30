using MongoDB.Driver;

namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

/// <summary>
/// Интерфейс фабрики для получения клиента MongoDb.
/// </summary>
/// <remarks>
/// Данный сервис уже зарегистрирован в DI, если вы подключили модуль WebApplicationModules.MongoDatabase.
/// Его нужно просто добавить в конструктор репозитория-наследника.
/// </remarks>
public interface IMongoClientFactory : IClientFactory<IMongoClient>
{
}
