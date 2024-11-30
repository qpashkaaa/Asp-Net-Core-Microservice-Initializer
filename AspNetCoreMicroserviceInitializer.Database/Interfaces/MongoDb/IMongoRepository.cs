namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

/// <summary>
/// Базовый интерфейс репозитория MongoDb.
/// </summary>
public interface IMongoRepository<TEntity> :
    IMongoReadRepository<TEntity>,
    IMongoWriteRepository<TEntity>
    where TEntity : IMongoEntity
{
}
