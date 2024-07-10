namespace AspNetCoreMicroserviceInitializer.Database.Interfaces;

/// <summary>
/// Интерфейс репозитория чтения.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IReadRepository<TEntity> 
    where TEntity : IEntity<long>
{
    /// <summary>
    /// Метод получения всех записей из таблицы.
    /// </summary>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Получить записи по идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{long}"/>.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<long> ids);

    /// <summary>
    /// Получить запись по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Модель типа <see cref="TEntity"/>, <see langword="null"/> если не найдена.</returns>
    Task<TEntity?> GetByIdAsync(long id);

    /// <summary>
    /// Получить записи с помощью пагинации.
    /// </summary>
    /// <param name="pageNumber">Номер строки.</param>
    /// <param name="pageSize">Количество записей.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    Task<IEnumerable<TEntity>> GetPageAsync(int pageNumber, int pageSize);

    /// <summary>
    /// Проверить существование элемента.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns><see langword="true"/> если элемент существует, <see langword="false"/> если элемент не существует.</returns>
    Task<bool> IsExistsAsync(long id);
}