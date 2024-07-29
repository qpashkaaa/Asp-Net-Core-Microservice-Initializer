using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AutoMapper;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;
using Hangfire.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

/// <summary>
/// Модули веб-приложения.
/// </summary>
public enum WebApplicationModules
{
    /// <summary>
    /// Модуль автоматической регистрации <see cref="AutoMapper"/>.
    ///
    /// Для корректной работы данного модуля необходимо:
    /// 1. Создать базовую модель.
    /// 2. Создать модель DTO.
    /// 3. Создать профиль, унаследованный от <see cref="Profile"/> для маппинга моделей.
    /// 4. Присвоить модели DTO атрибут <see cref="AutoRegisterProfileAttribute"/> и передать в его параметры необходимые типы моделей.
    /// </summary>
    /// <remarks>Регистрация моделей будет произведена автоматически, используя атрибут <see cref="AutoRegisterProfileAttribute"/>.</remarks>
    AutoMappers = 0,

    /// <summary>
    /// Модуль автоматической регистрации политики Cors.
    ///
    /// Для корректной работы данного модуля необходимо:
    /// 1. Создать в конфиге элемент модели настроек <see cref="CorsSettings"/>.
    /// </summary>
    Cors = 1,

    /// <summary>
    /// Модуль для работы с фоновыми задачами Hangfire.
    ///
    /// Для корректной работы данного модуля необходимо:
    /// 1. Создать фоновые задачи, реализующие интерфейс <see cref="IHangfireBackgroundTask"/>.
    /// 2. Создать в конфиге элементы моделей настроек задач, которые должны быть унаследованны от <see cref="HangfireTaskSettingsBase"/> для каждой задачи.
    /// 3. Создать в конфиге элемент модели настроек Hangfire <see cref="HangfireSettings"/>.
    /// 4. Создать в конфиге элемент модели настроек дашборда Hangfire <see cref="HangfireDashboardSettings"/>.
    /// 5. При необходимости создать фильтр авторизации для дашборда Hangfire, унаследованный от <see cref="IDashboardAuthorizationFilter"/> или же использовать существующие фильтры.
    /// 6. Присвоить задачам атрибут <see cref="AutoRegisterHangfireTaskAttribute"/> и передать в его параметры необходимый типы модели настроек.
    /// </summary>
    /// <remarks>Регистрация моделей будет произведена автоматически, используя атрибут <see cref="AutoRegisterHangfireTaskAttribute"/>.</remarks>
    Hangfire = 2,

    /// <summary>
    /// Модуль для автоматической регистрации HealthChecks.
    ///
    /// Для корректной работы данного модуля необходимо:
    /// 1. Добавить в приложение классы HealthChecks, унаследованные от <see cref="IHealthCheck"/> и присвоить им атрибут <see cref="AutoRegisterHealthCheckAttribute"/>.
    /// 2. Создать в конфиге элемент модели настроек Health Checks <see cref="HealthChecksSettings"/>.
    /// </summary>
    /// <remarks>1. Регистрация моделей будет произведена автоматически, используя реализацию интерфейса <see cref="IHealthCheck"/> и атрибут <see cref="AutoRegisterHealthCheckAttribute"/>.
    /// 2. Если в настройках конфига включен параметр <see cref="HealthChecksSettings.UIEnable"/>, то получить доступ к UI оболочке можно по URL: /healthchecks-ui</remarks>
    HealthChecks = 3,

    /// <summary>
    /// Модуль для автоматической регистрации настроек конфига.
    ///
    /// Для корректной работы данного модуля необходимо:
    /// 1. Добавить в приложение модель настроек конфига.
    /// 2. Создать в конфиге элемент модели настроек (название класса и название элемента конфига должны совпадать).
    /// 3. Присвоить моделям настроек атрибут <see cref="AutoRegisterConfigSettingsAttribute"/>.
    /// </summary>
    /// <remarks>Регистрация моделей будет произведена автоматически, используя атрибут <see cref="AutoRegisterConfigSettingsAttribute"/>.</remarks>
    Settings = 4,

    /// <summary>
    /// Метод добавления в приложение модуля для работы с базами данных.
    ///
    /// Для корректной работы данного модуля необходимо:
    /// 1. Создать модели <see cref="DbContext"/>.
    /// 2. Создать репозитории для работы с <see cref="DbContext"/>.
    /// 3. Присвоить моделям <see cref="DbContext"/> атрибут <see cref="AutoRegisterDbContextAttribute"/>.
    /// 4. Присвоить моделям репозиториев <see cref="AutoRegisterRepositoryAttribute"/>.
    /// </summary>
    /// <remarks>Регистрация моделей будет произведена автоматически, используя атрибуты <see cref="AutoRegisterDbContextAttribute"/> и <see cref="AutoRegisterRepositoryAttribute"/> (регистрация репозиториев производится как AddScoped).</remarks>
    Database = 5,
    
    /// <summary>
    /// Модуль Swagger.
    /// </summary>
    Swagger = 6,
    
    /// <summary>
    /// Модуль авторизации.
    /// </summary>
    Authorization = 7,
    
    /// <summary>
    /// Модуль контроллеров.
    /// </summary>
    Controllers = 8,

    /// <summary>
    /// Модуль Serilog.
    /// </summary>
    Serilog = 9,

    /// <summary>
    /// Модуль переменных окружения.
    /// </summary>
    EnvironmentVariables = 10,

    /// <summary>
    /// Модуль конфигурации ApiExplorer.
    /// </summary>
    EndpointsApiExplorer = 11,

    /// <summary>
    /// Модуль инициализации мигратора <see cref="Migrator"/> (применяет созданные миграции к БД) и проведения миграций при старте приложения с использованием <see cref="MigrationHostedService"/>.
    ///
    /// Для корректной работы мигратора необходимо:
    /// 1. Создать модели <see cref="DbContext"/>.
    /// 2. Присвоить моделям <see cref="DbContext"/> атрибут <see cref="AutoRegisterDbContextAttribute"/>.
    /// 3. Создать миграции, используя команду <code>dotnet ef migrations add InitialCreate --project your-project/your-project.csproj --startup-project your-project/your-project.csproj --output-dir Migrations</code>.
    /// </summary>
    Migrations = 12
}