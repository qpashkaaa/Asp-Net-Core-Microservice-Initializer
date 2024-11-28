using AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;

/// <summary>
/// Абстрактный класс базового Sql репозитория.
/// </summary>
public abstract class SqlRepositoryBase<TEntity> : ISqlRepository<TEntity>
    where TEntity : class, ISqlEntity<long>
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    protected readonly DbContext _dbContext;

    /// <summary>
    /// Конструктор <see cref="SqlRepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="dbContext">Контекст БД.</param>
    protected SqlRepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Метод получения всех записей из таблицы.
    /// </summary>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    /// <summary>
    /// Получить запись по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Модель типа <see cref="TEntity"/>, <see langword="null"/> если не найдена.</returns>
    public virtual async Task<TEntity?> GetByIdAsync(long id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    /// <summary>
    /// Получить записи по идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{long}"/>.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(
        IEnumerable<long> ids,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .Where(e => ids.Contains(e.Id))
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
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .OrderBy(entity => entity.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Проверить существование элемента.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns><see langword="true"/> если элемент существует, <see langword="false"/> если элемент не существует.</returns>
    public virtual async Task<bool> IsExistsAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .AnyAsync(e => e.Id == id, cancellationToken);
    }

    /// <summary>
    /// Метод удаления записи по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    public virtual async Task DeleteByIdAsync(
        long id,
        CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id);

        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbContext.Entry(entity).State = EntityState.Deleted;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Метод удаления записей по их идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{long}"/>.</param>
    /// <returns>Количество успешно удаленных записей.</returns>
    public virtual async Task<int> DeleteByIdsAsync(
        IEnumerable<long> ids,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<TEntity>()
            .Where(entity => ids.Contains(entity.Id))
            .ExecuteDeleteAsync(cancellationToken);
    }

    /// <summary>
    /// Метод вставки записи в таблицу.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    public virtual async Task InsertAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        entity.CreationDate = DateTime.UtcNow;

        await _dbContext.Set<TEntity>().AddAsync(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Метод вставки нескольких записей в таблицу.
    /// </summary>
    /// <param name="entities">Массив сущностей типа <see cref="IEnumerable{TEntity}"/>.</param>
    public virtual async Task InsertBatchAsync(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            entity.CreationDate = DateTime.UtcNow;
        }

        await _dbContext.Set<TEntity>().AddRangeAsync(entities);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Обновить запись в таблице.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    public virtual async Task UpdateAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await SetImmutableFields(entity);

        entity.LastUpdateDate = DateTime.UtcNow;

        _dbContext.Set<TEntity>().Update(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
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
            await SetImmutableFields(entity);

            entity.LastUpdateDate = DateTime.UtcNow;
        }

        _dbContext.Set<TEntity>().UpdateRange(entities);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Метод получения количества записей в таблице.
    /// </summary>
    /// <returns>Количество записей в таблице.</returns>
    public virtual async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<TEntity>()
            .CountAsync(cancellationToken);
    }

    /// <summary>
    /// Метод сохранения неизменяемых полей сущности.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    private async Task SetImmutableFields(TEntity entity)
    {
        var existEntity = await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == entity.Id);
        entity.CreationDate = existEntity?.CreationDate ?? throw new ArgumentNullException(nameof(existEntity));
    }
}