using MongoDB.Bson;

namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

/// <summary>
/// Интерфейс репозитория записи данных в MongoDb.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IMongoWriteRepository<TEntity>
    where TEntity : IMongoEntity
{
    /// <summary>
    /// Метод вставки записи в таблицу.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    Task InsertAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод вставки нескольких записей в таблицу.
    /// </summary>
    /// <param name="entities">Массив сущностей типа <see cref="IEnumerable{TEntity}"/>.</param>
    Task InsertBatchAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить запись в таблице.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    Task UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод обновления несколких записей в таблице.
    /// </summary>
    /// <param name="entities">Массив значений типа <see cref="IEnumerable{TEntity}"/>.</param>
    Task UpdateBatchAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод удаления записи по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    Task DeleteByIdAsync(
        ObjectId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод удаления записей по их идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{ObjectId}"/>.</param>
    /// <returns>Количество успешно удаленных записей.</returns>
    Task DeleteByIdsAsync(
        IEnumerable<ObjectId> ids,
        CancellationToken cancellationToken = default);
}
