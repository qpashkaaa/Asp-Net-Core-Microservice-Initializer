using AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

/// <summary>
/// Интерфейс базовой MongoDb сущности.
/// </summary>
public interface IMongoEntity : IEntity
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [BsonId]
    ObjectId Id { get; set; }
}
