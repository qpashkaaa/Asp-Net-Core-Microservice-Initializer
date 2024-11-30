using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using MongoDB.Driver;

namespace AspNetCoreMicroserviceInitializer.Database.Services.MongoDb;

/// <summary>
/// Фабрика для получения клиента MongoDb.
/// </summary>
public class MongoClientFactory : IMongoClientFactory
{
    /// <summary>
    /// Клиенты MongoDb.
    /// </summary>
    private readonly IEnumerable<IMongoClientWithConnectionString> _clients;

    /// <summary>
    /// Конструктор фабрики.
    /// </summary>
    /// <param name="clients">Клиенты MongoDb.</param>
    public MongoClientFactory(
        IEnumerable<IMongoClientWithConnectionString> clients)
    {
        _clients = clients;
    }

    /// <summary>
    /// Метод получения клиента по строке подключения.
    /// </summary>
    /// <param name="connectionString">Строка подключения.</param>
    /// <returns>Клиент типа <see cref="IMongoClient"/>.</returns>
    public IMongoClient GetClientByConnectionString(string connectionString)
    {
        return _clients
            .Where(client => client.GetConnectionString() == connectionString)
            .First()
            .GetClient();
    }
}
