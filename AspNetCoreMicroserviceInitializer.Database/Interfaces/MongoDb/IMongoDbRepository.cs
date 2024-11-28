namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

/// <summary>
/// Базовый интерфейс репозитория MongoDb.
/// </summary>
public interface IMongoDbRepository<TEntity> :
    IMongoDbReadRepository<TEntity>,
    IMongoDbWriteRepository<TEntity>
    where TEntity : IMongoDbEntity
{
}
