using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

/// <summary>
/// Атрибут для автоматической регистрации сервисов в DI.
/// </summary>
/// <remarks> Не предназначен для использования с абстрактными классами.</remarks>
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterServiceAttribute : AutoRegisterServiceBaseAttribute
{
    /// <summary>
    /// Конструктор атрибута.
    /// </summary>
    /// <param name="serviceLifetime">Тип жизненного цикла сервиса.</param>
    /// <param name="interfaceType">Тип интерфейса (сервиса).</param>
    public AutoRegisterServiceAttribute(
        ServiceLifetime serviceLifetime,
        Type? interfaceType = null) : base(serviceLifetime, interfaceType)
    {
    }
}
