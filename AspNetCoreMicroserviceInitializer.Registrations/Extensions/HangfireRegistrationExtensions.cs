using System.Reflection;
using AspNetCoreMicroserviceInitializer.Registrations.Builders;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для регистрации планировщика задач Hangfire.
/// </summary>
internal static class HangfireRegistrationExtensions
{
    /// <summary>
    /// Метод регистрации фоновых задач Hangfire.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    internal static IServiceCollection AddHangfireTasks(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        
        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterHangfireTaskAttribute>(false, serviceProvider);
        
        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<AutoRegisterHangfireTaskAttribute>(false).Any()),
            type =>
            {
                services.AddTransient(type);
            });
        
        return services;
    }

    /// <summary>
    /// Метод добавления Hangfire в <see cref="IHostApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">Билдер.</param>
    internal static IHostApplicationBuilder AddHangfire(
        this IHostApplicationBuilder builder)
    {
        var hangfireSettings = builder.Configuration.GetSection(nameof(HangfireSettings)).Get<HangfireSettings>();
        
        var configureAction = ValidateHangfireSettings(hangfireSettings);
        
        builder.Services.AddHangfire(configureAction);

        builder.Services.AddHangfireServer(options =>
        {
            options.SchedulePollingInterval = TimeSpan.FromSeconds(1);
        });

        return builder;
    }

    /// <summary>
    /// Метод подключения дашборда Hangfire с кастомными фильтрами авторизации, если они указаны в конфиге.
    /// Если фильтры не указаны, используется авторизация Hangfire по умолчанию.
    /// </summary>
    /// <param name="app">Билдер приложения.</param>
    internal static IApplicationBuilder UseHangfireDashboard(
        this IApplicationBuilder app)
    {
        var applicationServices = app.ApplicationServices;
        var serviceProvider = applicationServices as ServiceProvider ?? throw new ArgumentNullException(nameof(applicationServices));

        var hangfireDashboardSettings = applicationServices
            .GetService<IConfiguration>()?
            .GetSection(nameof(HangfireDashboardSettings))
            .Get<HangfireDashboardSettings>();

        var dashboardOptions = new DashboardOptions();

        if (hangfireDashboardSettings != null &&
            hangfireDashboardSettings.EnableCustomAuthorization &&
            !string.IsNullOrEmpty(hangfireDashboardSettings.FilterName))
        {
            var filterInstance = CreateFilterInstance(hangfireDashboardSettings.FilterName, serviceProvider);

            if (filterInstance != null)
            {
                dashboardOptions.Authorization = new[ ] { filterInstance };
            }
        }

        app.UseHangfireDashboard(options: dashboardOptions);

        return app;
    }

    /// <summary>
    /// Метод добавления фоновых задач Hangfire в <see cref="IApplicationBuilder"/>.
    /// </summary>
    /// <param name="app">Билдер приложения.</param>
    internal static IApplicationBuilder UseHangfireTasks(
        this IApplicationBuilder app)
    {
        var applicationServices = app.ApplicationServices;
        var serviceProvider = applicationServices as ServiceProvider ?? throw new ArgumentNullException(nameof(applicationServices));

        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterHangfireTaskAttribute>(false, serviceProvider);
        
        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<AutoRegisterHangfireTaskAttribute>(false).Any()),
            type =>
            {
                var attribute = type.GetCustomAttribute<AutoRegisterHangfireTaskAttribute>() ?? throw new Exception($"У модели {type.Name} не указан атрибут {nameof(AutoRegisterHangfireTaskAttribute)} для автоматической регистрации фоновой задачи.");
                    
                var addTaskMethod = typeof(HangfireRegistrationExtensions)
                    .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
                    .FirstOrDefault(m => m.Name == nameof(AddTask))?
                    .MakeGenericMethod(type, attribute.SettingsType);

                addTaskMethod?.Invoke(null, new[ ] { app } );
            });

        return app;
    }

    /// <summary>
    /// Метод добавления одной задачи в билдер приложения.
    /// </summary>
    /// <typeparam name="TTask">Тип задачи.</typeparam>
    /// <typeparam name="TTaskSettings">Тип настроек для задачи.</typeparam>
    /// <param name="app">Билдер приложения.</param>
    private static IApplicationBuilder AddTask<TTask, TTaskSettings>(this IApplicationBuilder app) 
        where TTask : IHangfireBackgroundTask
        where TTaskSettings : HangfireTaskSettingsBase
    {
        var task = app.ApplicationServices.GetRequiredService<TTask>();
        var test = typeof(TTaskSettings).Name;
        var taskSettings = app.ApplicationServices
            .GetService<IConfiguration>()?
            .GetSection(typeof(TTaskSettings).Name)
            .Get<TTaskSettings>();
        
        if (taskSettings == null)
        {
            throw new Exception($"В файле конфигурации не найдены настройки для задачи {typeof(TTask).Name} (task settings type: {typeof(TTaskSettings).Name}). Если элементов настроек в конфигурации нет, то заполните конфигурацию вручную или вызовите метод {nameof(WebApplicationFacade.InitBaseConfig)} у {nameof(WebApplicationFacade)} (перед вызовом метода необходимо в конструктор {nameof(WebApplicationFacade)} добавить модуль {WebApplicationModules.Settings} и присвоить модели {typeof(TTaskSettings).Name} атрибут {nameof(AutoRegisterConfigSettingsAttribute)}), заполните соответствующие данные в конфиге appsettings.json и перезапустите приложение.");
        }

        var taskName = typeof(TTask).Name;
        var recurringJobOptions = new RecurringJobOptions
        {
            TimeZone = TimeZoneInfo.FindSystemTimeZoneById(taskSettings.TimeZone)
        };

        RecurringJob.RemoveIfExists(taskName);

        RecurringJob.AddOrUpdate(
            taskName,
            () => task.ExecuteAsync(),
            taskSettings.CronExpression,
            recurringJobOptions);

        return app;
    }

    /// <summary>
    /// Создать экзмепляр класса, который реализует <see cref="IDashboardAuthorizationFilter"/> по имени.
    /// </summary>
    /// <param name="className">Имя класса.</param>
    /// <returns>Экземпляр класса, который реализует <see cref="IDashboardAuthorizationFilter"/>.</returns>
    private static IDashboardAuthorizationFilter? CreateFilterInstance(
        string className,
        ServiceProvider serviceProvider)
    {
        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificType<IDashboardAuthorizationFilter>(serviceProvider);
        
        foreach (var assembly in assemblies)
        {
            if (assembly != null)
            {
                var type = assembly.GetTypes().FirstOrDefault(t => t.Name == className);

                if (type != null)
                {
                    return (IDashboardAuthorizationFilter?)Activator.CreateInstance(type);
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Метод валидации настроек конфига Hangfire.
    /// </summary>
    /// <param name="hangfireSettings">Модель настроек Hangfire.</param>
    /// <returns>Action для конфигурации Hangfire.</returns>
    private static Action<IGlobalConfiguration> ValidateHangfireSettings(
        HangfireSettings? hangfireSettings = null)
    {
        if (hangfireSettings == null)
        {
            throw new ArgumentNullException(nameof(hangfireSettings));
        }
        
        switch (hangfireSettings.StorageType)
        {
            case HangfireStorage.Undefined:
                throw new ArgumentException($"В конфиге не указан тип хранилища для информации о задачах Hangfire. Если элементов настроек в конфигурации нет, то заполните конфигурацию вручную или вызовите метод {nameof(WebApplicationFacade.InitBaseConfig)} у {nameof(WebApplicationFacade)}, заполните соответствующие данные в конфиге appsettings.json и перезапустите приложение.", nameof(hangfireSettings.StorageType));
            case HangfireStorage.PostgreSql:
                if (string.IsNullOrEmpty(hangfireSettings.PostgreSqlConnectionString))
                {
                    throw new ArgumentNullException(nameof(hangfireSettings.PostgreSqlConnectionString));
                }
                
                return configuration =>
                {
                    configuration
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UsePostgreSqlStorage(connection =>
                            connection.UseNpgsqlConnection(hangfireSettings.PostgreSqlConnectionString));
                };
            case HangfireStorage.InMemory:
                return configuration =>
                {
                    configuration
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseMemoryStorage();
                };
            default:
                throw new ArgumentException("В конфиге не указан некорректный тип хранилища для информации о задачах Hangfire", nameof(hangfireSettings.StorageType));
        }
    }
}