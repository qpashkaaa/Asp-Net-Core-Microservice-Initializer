namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;

/// <summary>
/// Интерфейс базовой Sql сущности.
/// </summary>
/// <typeparam name="TId">Тип Id.</typeparam>
public interface ISqlEntity<TId> : IEntity
    where TId : struct
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    TId Id { get; set; }
}
