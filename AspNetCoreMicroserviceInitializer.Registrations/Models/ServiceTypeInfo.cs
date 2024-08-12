using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.Registrations.Models;

/// <summary>
/// Параметры сервиса, который регистрируется в DI.
/// </summary>
internal class ServiceTypeInfo
{
    /// <summary>
    /// Тип сервиса.
    /// </summary>
    public required Type ServiceType { get; set; }

    /// <summary>
    /// Количество параметров конструктора класса.
    /// </summary>
    public int ConstructorsParamsCount { get; set; }

    /// <summary>
    /// Тип жизненного цикла сервиса.
    /// </summary>
    public ServiceLifetime Lifetime { get; set; }

    /// <summary>
    /// Тип интерфейса (сервиса).
    /// </summary>
    /// <remarks>Должен быть интерфейсом.</remarks>
    public Type? InterfaceType;

    /// <summary>
    /// Фабричная функция для создания экземпляра сервиса внутри Dependency Injection контейнера.
    /// </summary>
    public Func<IServiceProvider, object>? ImplementationFactory { get; set; }
}
