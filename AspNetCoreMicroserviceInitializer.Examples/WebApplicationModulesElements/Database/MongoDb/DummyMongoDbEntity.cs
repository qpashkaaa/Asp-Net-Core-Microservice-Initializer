using AspNetCoreMicroserviceInitializer.Database.Attributes;
using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Database.MongoDb;

[MongoCollection("TestCollection")]
public class DummyMongoDbEntity : IMongoDbEntity
{
    [BsonId]
    public ObjectId Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public bool IsDeleted { get; set; }
}
