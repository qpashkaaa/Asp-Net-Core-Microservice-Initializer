
namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;

/// <summary>
/// Интерфейс репозитория для чтения данных из Redis.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IRedisReadRepository<TEntity>
    where TEntity : IRedisEntity
{
    /// <summary>
    /// Получить записи по ключам.
    /// </summary>
    /// <param name="keys">Массив ключей типа <see cref="IEnumerable{string}"/>.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    Task<IEnumerable<TEntity>> GetByKeysAsync(
        IEnumerable<string> keys,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить запись по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Модель типа <see cref="TEntity"/>, <see langword="null"/> если не найдена.</returns>
    Task<TEntity?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Проверить существование элемента.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns><see langword="true"/> если элемент существует, <see langword="false"/> если элемент не существует.</returns>
    Task<bool> IsExistsAsync(
        string key,
        CancellationToken cancellationToken = default);
}
