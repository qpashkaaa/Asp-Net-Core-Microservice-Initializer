using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;

/// <summary>
/// Интерфейс добавления фабричной функции для создания экземпляра сервиса внутри Dependency Injection контейнера.
/// </summary>
/// <typeparam name="TService">Тип сервиса, к которому будет применяться фабричная функция при регистрации.</typeparam>
/// <remarks>Используется, если необходимо добавить фабричную функцию при автоматической регистрации сервиса с использованием атрибута <see cref="AutoRegisterServiceAttribute"/>.</remarks>
public interface IServiceImplementationFactory<TService>
{
    /// <summary>Фабричная функция для создания экземпляра сервиса внутри Dependency Injection контейнера.</summary>
    /// <returns>Фабричная функция <see cref="Func{IServiceProvider, TService}"/> для создания экземпляра сервиса внутри DI контейнера.</returns>
    /// <remarks>
    /// Если используется атрибут <see cref="AutoRegisterServiceAttribute"/> и при регистрации сервиса необходимо использовать фабричный метод для создания экземпляра сервиса 
    /// (<see cref="ServiceCollectionServiceExtensions.AddTransient{TService, TImplementation}(IServiceCollection, Func{IServiceProvider, TImplementation})"/> или подобные методы),
    /// то реализуйте данный интерйес.
    /// </remarks>
    public Func<IServiceProvider, TService> ImplementationFactory { get; }
}
