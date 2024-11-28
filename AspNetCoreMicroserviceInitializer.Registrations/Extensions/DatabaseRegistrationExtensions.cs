using System.Reflection;
using AspNetCoreMicroserviceInitializer.Registrations.Helpers;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для регистрации зависимостей БД.
/// </summary>
internal static class DatabaseRegistrationExtensions
{
    /// <summary>
    /// Добавление сервисов БД (контексты БД и репозитории).
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекцию сервисов с зарегистрированными зависимостями.</returns>
    internal static IServiceCollection AddDbServices(
        this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        
        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterDbContextAttribute>(false, serviceProvider).ToList();
        
        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<AutoRegisterDbContextAttribute>(false).Any()),
            type =>
            {
                if (!type.IsSubclassOf(typeof(DbContext)))
                {
                    throw new Exception($"Невозможно применить атрибут {nameof(AutoRegisterDbContextAttribute)} к модели типа {type.FullName}. Атрибут {nameof(AutoRegisterDbContextAttribute)} может применяться только к профилям, которые наследуют {typeof(DbContext)}");
                }

                var addDbContextMethod = typeof(EntityFrameworkServiceCollectionExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.Public)
                    .FirstOrDefault(m => m.Name == "AddDbContext")?
                    .MakeGenericMethod(type);

                addDbContextMethod?.Invoke(null, new[] { services, null, (object)ServiceLifetime.Scoped, (object)ServiceLifetime.Scoped });
            });

        var repositoriesInfo = ServicesRegistrationHelper
            .GetOrderedServicesTypes<AutoRegisterRepositoryAttribute>(services);

        foreach (var serviceInfo in repositoriesInfo)
        {
            ServicesRegistrationHelper
                .RegisterService(services, serviceInfo);
        }

        return services;
    }
}