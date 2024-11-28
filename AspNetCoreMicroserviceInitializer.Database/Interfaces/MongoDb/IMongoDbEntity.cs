using AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

/// <summary>
/// Интерфейс базовой MongoDb сущности.
/// </summary>
/// <typeparam name="TId">Тип Id.</typeparam>
public interface IMongoDbEntity : IEntity
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [BsonId]
    ObjectId Id { get; set; }
}
