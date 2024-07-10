using AutoMapper;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

/// <summary>
/// Атрибут автоматической регистрации профилей <see cref="Profile"/> в маппере <see cref="IMapper"/>.
/// </summary>
/// <remarks> Не предназначен для использования с абстрактными классами.</remarks>
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterProfileAttribute : Attribute
{
}