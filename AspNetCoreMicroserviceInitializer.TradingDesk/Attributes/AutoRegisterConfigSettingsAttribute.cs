namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

/// <summary>
/// Атрибут для автоматической регистрации настроек конфига в DI для дальнейшего получения их с использованием <see cref="IOptions"/>.
/// </summary>
/// <remarks> Не предназначен для использования с абстрактными классами.</remarks>
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterConfigSettingsAttribute : Attribute
{
}