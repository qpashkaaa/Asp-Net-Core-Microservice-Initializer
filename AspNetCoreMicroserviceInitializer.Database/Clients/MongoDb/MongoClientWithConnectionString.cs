using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using MongoDB.Driver;

namespace AspNetCoreMicroserviceInitializer.Database.Clients.MongoDb;

/// <summary>
/// Интерфейс клиента MongoDb, который хранит в себе строку подключения.
/// </summary>
public class MongoClientWithConnectionString : IMongoClientWithConnectionString
{
    /// <summary>
    /// Строка подключения.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Клинет MongoDb.
    /// </summary>
    private readonly IMongoClient _mongoClient;

    /// <summary>
    /// Конструктор клиента.
    /// </summary>
    /// <param name="connectionString">Строка подключения</param>
    public MongoClientWithConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        _mongoClient = new MongoClient(connectionString);
    }

    /// <summary>
    /// Метод получения клиента MongoDb.
    /// </summary>
    /// <returns>Клиент MongoDb типа <see cref="IMongoClient"/>.</returns>
    public IMongoClient GetClient()
    {
        return _mongoClient;
    }

    /// <summary>
    /// Метод получения строки подключения MongoDb.
    /// </summary>
    /// <returns>Строка подключения MongoDb.</returns>
    public string GetConnectionString()
    {
        return _connectionString;
    }
}
