namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

/// <summary>
/// Атрибут для автоматической регистрации репозиториев в DI.
/// </summary>
/// <remarks> Не предназначен для использования с абстрактными классами.</remarks>
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterRepositoryAttribute : Attribute
{
}