using MongoDB.Driver;

namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

/// <summary>
/// Интерфейс клиента MongoDB со строкой подключения.
/// </summary>
public interface IMongoClientWithConnectionString : IClientWithConnectionString<IMongoClient>
{
}
