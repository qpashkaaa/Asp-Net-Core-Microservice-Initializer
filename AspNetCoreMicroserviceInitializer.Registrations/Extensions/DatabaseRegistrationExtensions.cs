using System.Reflection;
using AspNetCoreMicroserviceInitializer.Registrations.Helpers;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using AspNetCoreMicroserviceInitializer.Database.Clients.MongoDb;
using AspNetCoreMicroserviceInitializer.Database.Services.MongoDb;
using AspNetCoreMicroserviceInitializer.Database.Interfaces.Redis;
using AspNetCoreMicroserviceInitializer.Database.Clients.Redis;
using AspNetCoreMicroserviceInitializer.Database.Services.Redis;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для регистрации зависимостей БД.
/// </summary>
internal static class DatabaseRegistrationExtensions
{
    /// <summary>
    /// Добавление репозиториев для работы с БД.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекцию сервисов с зарегистрированными зависимостями.</returns>
    internal static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        var repositoriesInfo = ServicesRegistrationHelper
            .GetOrderedServicesTypes<AutoRegisterRepositoryAttribute>(services);

        foreach (var serviceInfo in repositoriesInfo)
        {
            ServicesRegistrationHelper
                .RegisterService(services, serviceInfo);
        }

        return services;
    }

    /// <summary>
    /// Добавление сервисов БД (контексты БД).
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекцию сервисов с зарегистрированными зависимостями.</returns>
    internal static IServiceCollection AddDbContexts(
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

        return services;
    }

    /// <summary>
    /// Добавление сервисов Redis.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекцию сервисов с зарегистрированными зависимостями.</returns>
    internal static IServiceCollection AddRedisServices(
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
                if (type.IsSubclassOf(typeof(RedisSettingsBase)))
                {
                    var configurationSection = configuration.GetSection(type.Name);

                    var redisSettings = configurationSection.Get(type) as RedisSettingsBase;

                    if (redisSettings != null)
                    {
                        services.AddSingleton<IRedisClientWithConnectionString>(sp =>
                        {
                            return new RedisClientWithConnectionString(redisSettings.ConnectionString);
                        });
                    }
                }
            });

        services.AddTransient<IRedisClientFactory, RedisClientFactory>();

        return services;
    }

    /// <summary>
    /// Добавление сервисов MongoDb.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Коллекцию сервисов с зарегистрированными зависимостями.</returns>
    internal static IServiceCollection AddMongoServices(
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
                if (type.IsSubclassOf(typeof(MongoSettingsBase)))
                {
                    var configurationSection = configuration.GetSection(type.Name);

                    var mongoSettings = configurationSection.Get(type) as MongoSettingsBase;

                    if (mongoSettings != null)
                    {
                        services.AddSingleton<IMongoClientWithConnectionString>(sp =>
                        {
                            return new MongoClientWithConnectionString(mongoSettings.ConnectionString);
                        });
                    }
                }
            });

        services.AddTransient<IMongoClientFactory, MongoClientFactory>();

        return services;
    }
}