using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

/// <summary>
/// Атрибут для метчинга настройки конфига и модуля приложения.
/// </summary>
/// <remarks>Внутренний атрибут, используется для аттрибутов, расположенных в <see cref="AspNetCoreMicroserviceInitializer.TradingDesk.Attributes"/>, 
/// чтобы понимать, какую настройку конфига добавлять в appsettings.json в случае использования метода базовой инициализации конфига.</remarks>
[AttributeUsage(AttributeTargets.Class)]
internal class ConfigSettingsModuleAttribute : Attribute
{
    /// <summary>
    /// Модуль, для которого используется данная настройка.
    /// </summary>
    public readonly WebApplicationModules WebApplicationModule;

    /// <summary>
    /// Конструктор атрибута.
    /// </summary>
    /// <param name="webApplicationModule">Модуль типа <see cref="WebApplicationModules"/>, для которого используется данная настройка.</param>
    public ConfigSettingsModuleAttribute(WebApplicationModules webApplicationModule)
    {
        WebApplicationModule = webApplicationModule;
    }
}
