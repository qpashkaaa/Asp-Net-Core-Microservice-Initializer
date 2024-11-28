namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;

/// <summary>
/// Интерфейс базового репозитория.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface ISqlRepository<TEntity> :
    ISqlReadRepository<TEntity>,
    ISqlWriteRepository<TEntity>
    where TEntity : ISqlEntity<long>
{
}