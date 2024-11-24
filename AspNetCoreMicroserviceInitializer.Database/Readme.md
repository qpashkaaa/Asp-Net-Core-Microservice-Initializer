# What It This?
  **A library with basic repository models, interfaces and the IEntity database entity.**
  >*This library contains the basic interfaces for working with databases.*

  >*It is assumed that the library will be used in conjunction with other libraries to quickly deploy microservices: "**AspNetCoreMicroserviceInitializer.TradingDesk**" and "**AspNetCoreMicroserviceInitializer.Registrations**".*

  >*For more information, see https://github.com/qpashkaaa/Asp-Net-Core-Microservice-Initializer*

## Functional features
- **Various interfaces for working with the database.**
```C#
/// <summary>
/// Интерфейс базовой сущности.
/// </summary>
/// <typeparam name="TId">Тип Id.</typeparam>
public interface IEntity<TId> where TId : struct
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    TId Id { get; set; }

    /// <summary>
    /// Дата создания сущности.
    /// </summary>
    DateTime CreationDate { get; set; }
}
```

```C#
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
```

```C#
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
```

```C#
/// <summary>
/// Интерфейс базового репозитория.
/// </summary>
/// <typeparam name="TEntity">Тип сущности.</typeparam>
public interface IRepository<TEntity> :
    IReadRepository<TEntity>,
    IWriteRepository<TEntity>
    where TEntity : IEntity<long>
{
}
```

- **The base class of an abstract relational database repository.**
```C#
/// <summary>
/// Абстрактный класс базового репозитория.
/// </summary>
public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
    where TEntity : class, IEntity<long>
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    private readonly DbContext _dbContext;

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
```

## Tech Stack
- **.NET 8**

## NuGet Packages
- ```Microsoft.EntityFrameworkCore```
- ```Microsoft.EntityFrameworkCore.Design```
- ```Microsoft.EntityFrameworkCore.Relational```
- ```Microsoft.EntityFrameworkCore.Tools```

## Authors
- [Pavel Roslyakov](https://github.com/qpashkaaa)

## Contacts
- [Portfolio Website](https://portfolio-website-qpashkaaa.vercel.app/)
- [Telegram](https://t.me/qpashkaaa)
- [VK](https://vk.com/qpashkaaa)
- [LinkedIN](https://www.linkedin.com/in/pavel-roslyakov-7b303928b/)

## Year of Development
> *2024*
  
