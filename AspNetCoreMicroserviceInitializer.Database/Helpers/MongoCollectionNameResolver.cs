using System.Reflection;
using AspNetCoreMicroserviceInitializer.Database.Attributes;
using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;

namespace AspNetCoreMicroserviceInitializer.Database.Helpers;

/// <summary>
/// Хелпер для определения имени коллекции MongoDb.
/// </summary>
internal static class MongoCollectionNameResolver
{
    /// <summary>
    /// Метод получения имени коллекции по типу модели или атрибуту <see cref="MongoCollectionAttribute"/>.
    /// </summary>
    /// <typeparam name="TEntity">Тип модели.</typeparam>
    /// <returns>Коллекция в формате строки.</returns>
    public static string GetCollectionName<TEntity>()
        where TEntity : IMongoDbEntity
    {
        var type = typeof(TEntity);

        var attribute = type.GetCustomAttribute<MongoCollectionAttribute>();

        if (attribute != null)
        {
            return attribute.CollectionName;
        }

        return type.Name;
    }
}
