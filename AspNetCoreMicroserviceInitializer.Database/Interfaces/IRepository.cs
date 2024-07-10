namespace AspNetCoreMicroserviceInitializer.Database.Interfaces;

/// <summary>
/// Интерфейс базового репозитория.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IRepository<TEntity> :
    IReadRepository<TEntity>,
    IWriteRepository<TEntity>
    where TEntity : IEntity<long>
{
}