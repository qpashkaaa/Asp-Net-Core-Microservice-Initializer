using AspNetCoreMicroserviceInitializer.TradingDesk.Exceptions;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes.Abstract;

/// <summary>
/// Абстрактный атрибут для автоматической регистрации сервисов в DI.
/// </summary>
/// <remarks> Не предназначен для использования с абстрактными классами.</remarks>
[AttributeUsage(AttributeTargets.Class)]
public abstract class AutoRegisterServiceBaseAttribute : Attribute
{
    /// <summary>
    /// Тип жизненного цикла сервиса.
    /// </summary>
    public readonly ServiceLifetime ServiceLifetime;

    /// <summary>
    /// Тип интерфейса (сервиса).
    /// </summary>
    /// <remarks>Должен быть интерфейсом. Если необходимо зарегистрировать сервис, без использования типа интерфейса, то не нужно передавать данный параметр.</remarks>
    public readonly Type? IntefaceType;

    /// <summary>
    /// Конструктор атрибута.
    /// </summary>
    /// <param name="serviceLifetime">Тип жизненного цикла сервиса.</param>
    /// <param name="interfaceType">Тип интерфейса (сервиса).</param>
    public AutoRegisterServiceBaseAttribute(
        ServiceLifetime serviceLifetime,
        Type? interfaceType = null)
    {
        ValidateProperties(interfaceType);

        ServiceLifetime = serviceLifetime;
        IntefaceType = interfaceType;
    }

    /// <summary>
    /// Метод валидации свойств атрибута.
    /// </summary>
    /// <param name="interfaceType">Тип интерфейса (сервиса).</param>
    private void ValidateProperties(
        Type? interfaceType = null)
    {
        if (interfaceType != null &&
            !interfaceType.IsInterface)
        {
            throw new AttributeException($"{nameof(interfaceType)} не является интерфейсом. Если Вы хотите зарегистрировать класс без использования интерфейса, то передайте параметр {nameof(interfaceType)} как null.");
        }

        if (interfaceType == typeof(IServiceImplementationFactory<>))
        {
            throw new AttributeException($"{typeof(IServiceImplementationFactory<>)} не может использоваться в качестве типа сервиса в атрибуте {typeof(AutoRegisterServiceAttribute)}. Он предназначен только для переопределения фабричной функции регистрации сервиса.");
        }
    }
}
