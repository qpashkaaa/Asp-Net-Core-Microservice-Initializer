namespace AspNetCoreMicroserviceInitializer.Database.Interfaces.Sql;

public interface IEntity
{
    /// <summary>
    /// Дата создания сущности.
    /// </summary>
    DateTime CreationDate { get; set; }

    /// <summary>
    /// Дата последнего обновления сущности.
    /// </summary>
    DateTime LastUpdateDate { get; set; }

    /// <summary>
    /// Флаг показывающий, удалена ли сущность.
    /// </summary>
    bool IsDeleted { get; set; }
}