namespace AspNetCoreMicroserviceInitializer.Database.Interfaces;

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