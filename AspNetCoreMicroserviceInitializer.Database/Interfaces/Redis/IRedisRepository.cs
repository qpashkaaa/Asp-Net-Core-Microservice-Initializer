namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;

/// <summary>
/// Интерфейс базового репозитория Redis.
/// </summary>
public interface IRedisRepository<TEntity> : 
    IRedisReadRepository<TEntity>, 
    IRedisWriteRepository<TEntity>
    where TEntity : IRedisEntity
{
}
