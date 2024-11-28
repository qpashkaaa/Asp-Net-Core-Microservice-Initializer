namespace AspNetCoreMicroserviceInitializer.Database.Attributes;

/// <summary>
/// Атрибут для указания имени коллекции в строчном формате.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MongoCollectionAttribute : Attribute
{
    /// <summary>
    /// Имя коллекции.
    /// </summary>
    public string CollectionName { get; }

    /// <summary>
    /// Конструктор атрибута.
    /// </summary>
    /// <param name="collectionName">Имя коллекции.</param>
    public MongoCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}
