using AspNetCoreMicroserviceInitializer.Database.Helpers;
using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;

/// <summary>
/// Абстрактный класс базового MongoDb репозитория.
/// </summary>
public abstract class MongoDbRepositoryBase<TEntity> : IMongoDbRepository<TEntity>
    where TEntity : class, IMongoDbEntity
{
    /// <summary>
    /// База данных MongoDb.
    /// </summary>
    protected readonly IMongoDatabase _database;

    /// <summary>
    /// Коллекция MongoDb.
    /// </summary>
    protected readonly IMongoCollection<TEntity> _collection;

    /// <summary>
    /// Конструктор <see cref="MongoDbRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="database">База данных MongoDb.</param>
    protected MongoDbRepositoryBase(IMongoDatabase database)
    {
        _database = database;

        var collectionName = MongoCollectionNameResolver.GetCollectionName<TEntity>();

        _collection = _database.GetCollection<TEntity>(collectionName);
    }


    /// <summary>
    /// Метод получения всех записей из таблицы.
    /// </summary>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(Builders<TEntity>.Filter.Empty)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получить запись по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Модель типа <see cref="TEntity"/>, <see langword="null"/> если не найдена.</returns>
    public virtual async Task<TEntity?> GetByIdAsync(
        ObjectId id,
        CancellationToken cancellationToken = default)
    {
        return await _collection
            .Find(entity => entity.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Получить записи по идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{long}"/>.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(
        IEnumerable<ObjectId> ids,
        CancellationToken cancellationToken = default)
    {
        if (ids == null ||
            !ids.Any())
        {
            return Enumerable.Empty<TEntity>();
        }

        var filter = Builders<TEntity>.Filter.In(entity => entity.Id, ids);

        return await _collection
            .Find(filter)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Получить записи с помощью пагинации.
    /// </summary>
    /// <param name="pageNumber">Номер строки.</param>
    /// <param name="pageSize">Количество записей.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetPageAsync(
        int pageNumber, 
        int pageSize,
        FilterDefinition<TEntity>? filter = null,
        CancellationToken cancellationToken = default)
    {
        if (filter == null)
        {
            filter = Builders<TEntity>.Filter.Empty;
        }

        return await _collection
            .Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Метод получения количества записей в таблице.
    /// </summary>
    /// <returns>Количество записей в таблице.</returns>
    public virtual async Task<long> GetCountAsync(
        FilterDefinition<TEntity>? filter = null,
        CancellationToken cancellationToken = default)
    {
        if (filter is null)
        {
            filter = Builders<TEntity>.Filter.Empty;
        }

        return await _collection
            .CountDocumentsAsync(filter, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Проверить существование элемента.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns><see langword="true"/> если элемент существует, <see langword="false"/> если элемент не существует.</returns>
    public virtual async Task<bool> IsExistsAsync(
        ObjectId id,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.Eq(entity => entity.Id, id);
        var count = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        return count > 0;
    }

    /// <summary>
    /// Метод удаления записи по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    public virtual async Task DeleteByIdAsync(
        ObjectId id, 
        CancellationToken cancellationToken = default)
    {
        await _collection
            .DeleteOneAsync(entity => entity.Id == id, cancellationToken);
    }

    /// <summary>
    /// Метод удаления записей по их идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{long}"/>.</param>
    /// <returns>Количество успешно удаленных записей.</returns>
    public virtual async Task DeleteByIdsAsync(
        IEnumerable<ObjectId> ids, 
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<TEntity>.Filter.In(entity => entity.Id, ids);

        await _collection
            .DeleteManyAsync(filter, cancellationToken);
    }

    /// <summary>
    /// Метод вставки записи в таблицу.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    public virtual async Task InsertAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        await _collection
            .InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Метод вставки нескольких записей в таблицу.
    /// </summary>
    /// <param name="entities">Массив сущностей типа <see cref="IEnumerable{TEntity}"/>.</param>
    public virtual async Task InsertBatchAsync(
        IEnumerable<TEntity> entities, 
        CancellationToken cancellationToken = default)
    {
        await _collection
            .InsertManyAsync(entities, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Обновить запись в таблице.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    public virtual async Task UpdateAsync(
        TEntity entity, 
        CancellationToken cancellationToken)
    {
        await _collection.ReplaceOneAsync(
            e => e.Id == entity.Id, entity,
            cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Метод обновления несколких записей в таблице.
    /// </summary>
    /// <param name="entities">Массив значений типа <see cref="IEnumerable{TEntity}"/>.</param>
    public virtual async Task UpdateBatchAsync(
        IEnumerable<TEntity> entities, 
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await UpdateAsync(entity, cancellationToken);
        }
    }
}
