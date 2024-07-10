using System.Reflection;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для автоматической регистрации зависимостей настроек из конфига.
/// Автоматически регистрирует настройки конфига, у классов которых есть атрибут <see cref="AutoRegisterConfigSettingsAttribute"/>.
/// </summary>
internal static class SettingsRegistrationExtensions
{
    /// <summary>
    /// Добавление настроек из конфига.
    /// Автоматически регистрирует настройки конфига, у классов которых есть атрибут <see cref="AutoRegisterConfigSettingsAttribute"/>.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация.</param>
    internal static IServiceCollection AddSettings(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        var serviceProvider = services.BuildServiceProvider();

        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterConfigSettingsAttribute>(false, serviceProvider);
        
        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<AutoRegisterConfigSettingsAttribute>(false).Any()),
            type =>
            {
                var configureMethod = typeof(OptionsConfigurationServiceCollectionExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == "Configure" && m.GetParameters().Length == 2)?
                    .MakeGenericMethod(type);

                object configurationSection = configuration.GetSection(type.Name);

                configureMethod?.Invoke(null, new[] { services, configurationSection });
            });

        return services;
    }
}