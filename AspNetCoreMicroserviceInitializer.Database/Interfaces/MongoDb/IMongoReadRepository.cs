using MongoDB.Bson;
using MongoDB.Driver;

namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

/// <summary>
/// Интерфейс репозитория для чтения данных из MongoDB.
/// </summary>
public interface IMongoReadRepository<TEntity>
    where TEntity : IMongoEntity
{
    /// <summary>
    /// Метод получения всех записей из таблицы.
    /// </summary>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить записи по идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{ObjectId}"/>.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    Task<IEnumerable<TEntity>> GetByIdsAsync(
        IEnumerable<ObjectId> ids,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить запись по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Модель типа <see cref="TEntity"/>, <see langword="null"/> если не найдена.</returns>
    Task<TEntity?> GetByIdAsync(
        ObjectId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить записи с помощью пагинации.
    /// </summary>
    /// <param name="pageNumber">Номер строки.</param>
    /// <param name="pageSize">Количество записей.</param>
    /// <param name="filter">Фильтр для выборки (необязательный параметр).</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    Task<IEnumerable<TEntity>> GetPageAsync(
        int pageNumber,
        int pageSize,
        FilterDefinition<TEntity>? filter = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование элемента.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns><see langword="true"/> если элемент существует, <see langword="false"/> если элемент не существует.</returns>
    Task<bool> IsExistsAsync(
        ObjectId id,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Метод получения количества записей в таблице.
    /// </summary>
    /// <param name="filter">Фильтр для подсчета (необязательный параметр).</param>
    /// <returns>Количество записей в таблице.</returns>
    Task<long> GetCountAsync(
        FilterDefinition<TEntity>? filter = null,
        CancellationToken cancellationToken = default);
}
