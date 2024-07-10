namespace AspNetCoreMicroserviceInitializer.Database.Interfaces;

/// <summary>
/// Интерфейс репозитория записи.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IWriteRepository<TEntity>
    where TEntity : IEntity<long>
{
    /// <summary>
    /// Метод вставки записи в таблицу.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    Task InsertAsync(TEntity entity);

    /// <summary>
    /// Метод вставки нескольких записей в таблицу.
    /// </summary>
    /// <param name="entities">Массив сущностей типа <see cref="IEnumerable{TEntity}"/>.</param>
    Task InsertBatchAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Обновить запись в таблице.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Метод обновления несколких записей в таблице.
    /// </summary>
    /// <param name="entities">Массив значений типа <see cref="IEnumerable{TEntity}"/>.</param>
    Task UpdateBatchAsync(IEnumerable<TEntity> entities);

    /// <summary>
    /// Метод удаления записи по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    Task DeleteByIdAsync(long id);

    /// <summary>
    /// Метод удаления записей по их идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{long}"/>.</param>
    /// <returns>Количество успешно удаленных записей.</returns>
    Task<int> DeleteByIdsAsync(IEnumerable<long> ids);
}