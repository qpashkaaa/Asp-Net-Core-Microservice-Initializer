using AspNetCoreMicroserviceInitializer.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMicroserviceInitializer.Database.Repositories.Abstract;

/// <summary>
/// Абстрактный класс базового репозитория.
/// </summary>
public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity<long>
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    protected readonly DbContext _dbContext;

    /// <summary>
    /// Конструктор <see cref="RepositoryBase{TEntity}"/>.
    /// </summary>
    /// <param name="dbContext">Контекст БД.</param>
    protected RepositoryBase(DbContext dbContext)
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
    public virtual async Task<IEnumerable<TEntity>> GetByIdsAsync(IEnumerable<long> ids)
    {
        return await _dbContext.Set<TEntity>()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync();
    }

    /// <summary>
    /// Получить записи с помощью пагинации.
    /// </summary>
    /// <param name="pageNumber">Номер строки.</param>
    /// <param name="pageSize">Количество записей.</param>
    /// <returns>Коллекцию типа <see cref="IEnumerable{TEntity}"/>.</returns>
    public virtual async Task<IEnumerable<TEntity>> GetPageAsync(int pageNumber, int pageSize)
    {
        return await _dbContext.Set<TEntity>()
            .OrderBy(entity => entity.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    /// <summary>
    /// Проверить существование элемента.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns><see langword="true"/> если элемент существует, <see langword="false"/> если элемент не существует.</returns>
    public virtual async Task<bool> IsExistsAsync(long id)
    {
        return await _dbContext.Set<TEntity>()
            .AnyAsync(e => e.Id == id);
    }

    /// <summary>
    /// Метод удаления записи по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    public virtual async Task DeleteByIdAsync(long id)
    {
        var entity = await GetByIdAsync(id);

        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _dbContext.Entry(entity).State = EntityState.Deleted;
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Метод удаления записей по их идентификаторам.
    /// </summary>
    /// <param name="ids">Массив идентификаторов типа <see cref="IEnumerable{long}"/>.</param>
    /// <returns>Количество успешно удаленных записей.</returns>
    public virtual async Task<int> DeleteByIdsAsync(IEnumerable<long> ids)
    {
        return await _dbContext.Set<TEntity>()
            .Where(entity => ids.Contains(entity.Id))
            .ExecuteDeleteAsync();
    }

    /// <summary>
    /// Метод вставки записи в таблицу.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    public virtual async Task InsertAsync(TEntity entity)
    {
        entity.CreationDate = DateTime.UtcNow;

        await _dbContext.Set<TEntity>().AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Метод вставки нескольких записей в таблицу.
    /// </summary>
    /// <param name="entities">Массив сущностей типа <see cref="IEnumerable{TEntity}"/>.</param>
    public virtual async Task InsertBatchAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreationDate = DateTime.UtcNow;
        }

        await _dbContext.Set<TEntity>().AddRangeAsync(entities);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Обновить запись в таблице.
    /// </summary>
    /// <param name="entity">Сущность типа <see cref="TEntity"/>.</param>
    public virtual async Task UpdateAsync(TEntity entity)
    {
        await SetImmutableFields(entity);

        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Метод обновления несколких записей в таблице.
    /// </summary>
    /// <param name="entities">Массив значений типа <see cref="IEnumerable{TEntity}"/>.</param>
    public virtual async Task UpdateBatchAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            await SetImmutableFields(entity);
        }

        _dbContext.Set<TEntity>().UpdateRange(entities);
        await _dbContext.SaveChangesAsync();
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