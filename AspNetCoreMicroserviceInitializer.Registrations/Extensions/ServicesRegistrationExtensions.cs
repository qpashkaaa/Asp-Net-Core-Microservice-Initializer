using AspNetCoreMicroserviceInitializer.Registrations.Helpers;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для автоматической регистрации сервисов.
/// </summary>
/// <remarks>Автоматически регистрирует в DI сервисы, у которых есть атрибут <see cref="AutoRegisterServiceAttribute"/>.</remarks>
internal static class ServicesRegistrationExtensions
{
    /// <summary>
    /// Метод добавления сервисов в <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        var servicesInfo = ServicesRegistrationHelper
            .GetOrderedServicesTypes<AutoRegisterServiceAttribute>(services);

        foreach (var serviceInfo in servicesInfo)
        {
            ServicesRegistrationHelper
                .RegisterService(services, serviceInfo);
        }

        return services;
    }
}
