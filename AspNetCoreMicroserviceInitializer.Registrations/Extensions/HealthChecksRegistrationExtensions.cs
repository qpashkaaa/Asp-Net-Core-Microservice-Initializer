using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для регистрации Health Checks.
/// </summary>
internal static class HealthChecksRegistrationExtensions
{
    /// <summary>
    /// Метод добавления Health Checks.
    /// 
    /// <remarks>
    /// Информация: 
    /// Этот метод работает с классами, которые унаследованы от интерфейса <see cref="IHealthCheck"/>.
    /// Он с помощью рефлексии находит классы, которые реализуют интерфейс <see cref="IHealthCheck"/> и у которых есть атрибут <see cref="AutoRegisterHealthCheckAttribute"/>, регистрирует их в DI и регистрирует их как HealthChecks.
    /// 
    /// Важно:
    /// Не нужно самостоятельно регистрировать Health Checks в DI. Все выполнится автоматически.
    /// Достаточно просто создать класс Health Check и унаследовать его от <see cref="IHealthCheck"/> и присвоить атрибут <see cref="AutoRegisterHealthCheckAttribute"/>. Данное расширение все сделает автоматически.
    /// 
    /// Информация-2:
    /// Доп. параметры настраиваются конфигом appsettings. Параметры настраиваются в соответствии с моделью <see cref="HealthChecksSettings"/>.
    /// </remarks>
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    internal static IServiceCollection AddHealthChecks(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        services.AddHealthChecksSettings(configuration);
        
        var serviceProvider = services.BuildServiceProvider();
        var healthChecksSettings = serviceProvider.GetRequiredService<IOptions<HealthChecksSettings>>().Value;

        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterHealthCheckAttribute>(false, serviceProvider);

        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes()
                .Where(t => t.IsClass &&
                            !t.IsAbstract &&
                            t.GetInterfaces().Contains(typeof(IHealthCheck))),
            type =>
            {
                var healthCheckName = type.Name;

                var healthCheckInstance = Activator.CreateInstance(type);

                services.AddHealthChecks().AddCheck(healthCheckName, (IHealthCheck)healthCheckInstance!);
            });
        

        if (healthChecksSettings.UIEnable &&
            !string.IsNullOrEmpty(healthChecksSettings.UIFullUrl))
        {
            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint("Health Checks", healthChecksSettings.UIFullUrl);
                setup.SetEvaluationTimeInSeconds(healthChecksSettings.UIEvaluationTimeInSeconds ?? 30);
                setup.SetApiMaxActiveRequests(healthChecksSettings.UIApiMaxActiveRequests ?? 2);
            }).AddInMemoryStorage();
        }

        return services;
    }

    /// <summary>
    /// Метод подключения Health Checks.
    /// 
    /// <remarks>
    /// Информация: 
    /// Этот метод работает с классами, которые унаследованы от интерфейса <see cref="IHealthCheck"/>.
    /// Он с помощью рефлексии находит классы, которые реализуют интерфейс <see cref="IHealthCheck"/> и у которых есть атрибут <see cref="AutoRegisterHealthCheckAttribute"/>, регистрирует их в DI и регистрирует их как HealthChecks.
    /// 
    /// Важно:
    /// Не нужно самостоятельно регистрировать Health Checks в DI. Все выполнится автоматически.
    /// Достаточно просто создать класс Health Check и унаследовать его от <see cref="IHealthCheck"/> и присвоить атрибут <see cref="AutoRegisterHealthCheckAttribute"/>. Данное расширение все сделает автоматически.
    /// 
    /// Информация-2:
    /// Доп. параметры настраиваются конфигом appsettings. Параметры настраиваются в соответствии с моделью <see cref="HealthChecksSettings"/>.
    /// </remarks>
    /// </summary>
    /// <param name="app">Веб-приложение, используемое для настройки HTTP-конвейера и маршрутов.</param>
    internal static WebApplication UseHealthChecks(
        this WebApplication app)
    {
        var healthChecksSettings = app.Services.GetRequiredService<IOptions<HealthChecksSettings>>().Value;
        var healthChecksOptions = new HealthCheckOptions();

        if (healthChecksSettings.UIEnable &&
            !string.IsNullOrEmpty(healthChecksSettings.UIFullUrl))
        {
            app.UseHealthChecksUI();
            healthChecksOptions.ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse;
        }

        try
        {
            app.MapHealthChecks(healthChecksSettings.Endpoint, healthChecksOptions);
        }
        catch
        {
            throw new Exception($"Не найдены классы Health Checks при регистрации зависимостей. Пожалуйста, создайте экземпляр класса, реализующий интерфейс {typeof(IHealthCheck)} и присвойте ему атрибут {typeof(AutoRegisterHealthCheckAttribute)}.");
        }

        return app;
    }

    /// <summary>
    /// Добавление настроек Health Checks из конфига.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="configuration">Конфигурация.</param>
    private static IServiceCollection AddHealthChecksSettings(
        this IServiceCollection services,
        IConfigurationManager configuration)
    {
        services.Configure<HealthChecksSettings>(configuration.GetSection(nameof(HealthChecksSettings)));

        return services;
    }
}