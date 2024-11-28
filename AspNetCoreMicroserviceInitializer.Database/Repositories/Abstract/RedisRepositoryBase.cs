using StackExchange.Redis;
using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;

namespace AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;

/// <summary>
/// Абстрактный класс базового Redis репозитория.
/// </summary>
public abstract class RedisRepositoryBase<TEntity> : IRedisRepository<TEntity>
    where TEntity : class, IRedisEntity, new()
{
    /// <summary>
    /// База данных.
    /// </summary>
    protected readonly IDatabase _database;

    /// <summary>
    /// Префикс ключа для унификации.
    /// </summary>
    protected readonly string _keyPrefix;

    /// <summary>
    /// Модель для подключения к БД Redis.
    /// </summary>
    protected readonly IConnectionMultiplexer _multiplexer;

    /// <summary>
    /// Конструктор репозитория.
    /// </summary>
    /// <param name="multiplexer">Модель для подключения к БД Redis.</param>
    /// <param name="keyPrefix">Префикс ключа для унификации.</param>
    protected RedisRepositoryBase(
        IConnectionMultiplexer multiplexer, 
        string keyPrefix)
    {
        _multiplexer = multiplexer;
        _database = multiplexer.GetDatabase();
        _keyPrefix = keyPrefix;
    }

    /// <summary>
    /// Получить запись по ключу.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Модель типа <see cref="TEntity"/>, <see langword="null"/> если не найдена.</returns>
    public virtual async Task<TEntity?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var value = await _database.StringGetAsync(BuildKey(key));

        if (value.IsNullOrEmpty)
        {
            return await Task.FromResult<TEntity?>(null);
        }

        return new TEntity
        {
            Key = key,
            Value = value.ToString()
        };
    }

    /// <summary>
    /// Получить записи по ключам.
    /// </summary>
    /// <param name="keys">Массив ключей типа <see cref="IEnumerable{string}"/>.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetByKeysAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        var result = new List<TEntity>();

        foreach (var key in keys)
        {
            var entity = await GetByKeyAsync(key);

            if (entity is not null)
            {
                result.Add(entity);
            }
        }

        return result;
    }

    /// <summary>
    /// Проверить существование элемента.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns><see langword="true"/> если элемент существует, <see langword="false"/> если элемент не существует.</returns>
    public virtual async Task<bool> IsExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _database.KeyExistsAsync(BuildKey(key));
    }

    /// <summary>
    /// Метод вставки записи в таблицу.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _database.StringSetAsync(BuildKey(entity.Key), entity.Value);
    }

    /// <summary>
    /// Метод вставки нескольких записей в таблицу.
    /// </summary>
    /// <param name="entities">Массив сущностей типа <see cref="IEnumerable{TEntity}"/>.</param>
    public virtual async Task InsertBatchAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach(var entity in entities)
        {
            await InsertAsync(entity, cancellationToken);
        }
    }

    /// <summary>
    /// Обновить запись в таблице.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await InsertAsync(entity);
    }

    /// <summary>
    /// Метод обновления несколких записей в таблице.
    /// </summary>
    /// <param name="entities">Массив значений типа <see cref="IEnumerable{TEntity}"/>.</param>
    public virtual async Task UpdateBatchAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity, cancellationToken);
        }
    }

    /// <summary>
    /// Метод удаления записи по идентификатору.
    /// </summary>
    /// <param name="key">Идентификатор.</param>
    public virtual async Task DeleteByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        await _database.KeyDeleteAsync(BuildKey(key));
    }

    /// <summary>
    /// Метод удаления записей по их идентификаторам.
    /// </summary>
    /// <param name="keys">Массив идентификаторов типа <see cref="IEnumerable{ObjectId}"/>.</param>
    /// <returns>Количество успешно удаленных записей.</returns>
    public virtual async Task DeleteByKeysAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        var keysForDelete = keys
            .Select(key => (RedisKey)BuildKey(key))
            .ToArray();

        await _database.KeyDeleteAsync(keysForDelete);
    }

    /// <summary>
    /// Метод построения ключа с префиксом.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <returns>Ключ с префиксом</returns>
    private string BuildKey(string key)
    {
        return $"{_keyPrefix}:{key}";
    }
}
