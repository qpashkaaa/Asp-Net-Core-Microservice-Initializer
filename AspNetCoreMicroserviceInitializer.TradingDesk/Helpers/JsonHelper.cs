using System.Collections;
using System.Reflection;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;

/// <summary>
/// Класс хелперов для работы с Json.
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// Получить модель <see cref="JObject"/> из <see cref="Type"/>.
    /// </summary>
    /// <param name="type">Тип.</param>
    /// <returns>Модель типа в формате <see cref="JObject"/>.</returns>
    public static JObject GetModelJObject(Type type)
    {
        var instance = Activator.CreateInstance(type);
        return GetModelJObject(instance);
    }
    
    /// <summary>
    /// Получить модель <see cref="JObject"/> из инстнса <see cref="Type"/> типа <see cref="object"/>.
    /// </summary>
    /// <param name="instance">Инстанс типа.</param>
    /// <returns>Модель типа в формате <see cref="JObject"/>.</returns>
    public static JObject GetModelJObject(object instance)
    {
        var jObject = new JObject();
        var properties = instance
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;
            var propertyValue = property.GetValue(instance);
            var jsonPropertyNameAttribute = property.GetCustomAttribute<JsonPropertyNameAttribute>();
            var propertyName = jsonPropertyNameAttribute?.Name ?? property.Name;

            if (propertyType.IsArray)
            {
                jObject[propertyName] = GetModelJArray(propertyValue);
            }
            else if (propertyType.IsClass && propertyType != typeof(string))
            {
                jObject[propertyName] = propertyValue != null ? GetModelJObject(propertyValue) : JValue.CreateNull();
            }
            else
            {
                jObject[propertyName] = propertyValue != null ? JToken.FromObject(propertyValue) : JValue.CreateNull();
            }
        }

        return jObject;
    }
    
    /// <summary>
    /// Получить модель <see cref="JArray"/> из <see cref="IEnumerable"/> <see cref="propertyValue"/>.
    /// </summary>
    /// <param name="propertyValue">Значение свойства типа <see cref="IEnumerable"/>.</param>
    /// <returns>Модель типа в формате <see cref="JArray"/>.</returns>
    public static JArray GetModelJArray(object propertyValue)
    {
        var jArray = new JArray();

        if (propertyValue is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item != null)
                {
                    if (item.GetType().IsClass && item.GetType() != typeof(string))
                    {
                        jArray.Add(GetModelJObject(item));
                    }
                    else
                    {
                        jArray.Add(JToken.FromObject(item));
                    }
                }
                else
                {
                    jArray.Add(JValue.CreateNull());
                }
            }
        }

        return jArray;
    }
    
    /// <summary>
    /// Метод добавления недостающих свойств в Json.
    /// </summary>
    /// <param name="target">Существующая модель.</param>
    /// <param name="source">Новая модель.</param>
    public static void AddMissingProperties(JObject target, JObject source)
    {
        foreach (var property in source.Properties())
        {
            if (target[property.Name] == null)
            {
                target[property.Name] = property.Value;
            }
            else if (property.Value.Type == JTokenType.Object && target[property.Name] is JObject targetObject)
            {
                AddMissingProperties(targetObject, (JObject)property.Value);
            }
        }
    }
}