namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

/// <summary>
/// Атрибут для автоматической регистрации контекста БД.
/// </summary>
/// <remarks> Не предназначен для использования с абстрактными классами.</remarks>
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterDbContextAttribute : Attribute
{
}