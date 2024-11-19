# What It This?
  **A library for fast initialization of basic .NET 8 microservice modules and initialization of basic docker compose.**
  >*This library upgrade the existing WebApplicationBuilder and allows you to add the desired functionality to the application in just a couple of lines. This library using reflection and custom attributes to connect application modules, thereby encapsulating most of the code that is needed to initialize these modules in the microservice.*

# Important information
  **Before adding the library to your "ASP .NET 8 Core Web API" project it is necessary to remove the NuGet package "Swashbuckle.AspNetCore", which is added by default, from the project to avoid conflicts, since this package is available in the library "AspNetCoreMicroserviceInitializer.TradingDesk".**

## Functional features
  >*Additional examples of using the library's functionality in a real micro-service can be found in the project "**AspNetCoreMicroserviceInitializer.Examples**" of this repository.*

  >*Detailed information about each module and how to connect it is described in the summary of enum WebApplicationModules.cs.*
- **An example of creating a WebApplicationBuilder with the addition of all modules, creating a config and configuring a docker compose file.**
```C#
var builder = WebApplication.CreateBuilder(args);

// --------Creating a WebApplication with all possible parameters and passing a pre-configured WebApplicationBuilder (optional).--------

var app = new WebApplicationFacade(
modules: new HashSet<WebApplicationModules>
{
    WebApplicationModules.AutoMappers,
    WebApplicationModules.Cors,
    WebApplicationModules.Hangfire,
    WebApplicationModules.HealthChecks,
    WebApplicationModules.Settings,
    WebApplicationModules.Database,
    WebApplicationModules.Swagger,
    WebApplicationModules.Controllers,
    WebApplicationModules.Serilog,
    WebApplicationModules.EnvironmentVariables,
    WebApplicationModules.EndpointsApiExplorer,
    WebApplicationModules.Migrations,
    WebApplicationModules.Services
},
builder: builder)
    .AddAdditionalModules(builder =>
    {
        builder.Services.AddTransient<AdditionalGuidService>();
    })
    .AddAdditionalSerilogConfiguration((builder, serviceProvider, configuration) =>
    {
        configuration.Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", path =>
            "/health".Equals(path, StringComparison.OrdinalIgnoreCase)));
    })
    .InitBaseConfig(configPath: "appsettings.json")
    .InitBaseDockerComposeFiles(
    dockerComposeFileModules: new List<DockerComposeFileModules>
    {
        DockerComposeFileModules.Server,
        DockerComposeFileModules.Client,
        DockerComposeFileModules.Adminer,
        DockerComposeFileModules.MongoDb,
        DockerComposeFileModules.MongoExpress,
        DockerComposeFileModules.ClickHouse,
        DockerComposeFileModules.MySql,
        DockerComposeFileModules.Redis,
        DockerComposeFileModules.Elasticsearch,
        DockerComposeFileModules.Kibana,
        DockerComposeFileModules.Cassandra,
        DockerComposeFileModules.RabbitMq,
        DockerComposeFileModules.Prometheus,
        DockerComposeFileModules.Grafana,
        DockerComposeFileModules.Nginx,
        DockerComposeFileModules.PostgreSql
    },
    configPath: "appsettings.json")
    .CreateApplication();

// --------Creating a WebApplication with all possible parameters and passing a pre-configured WebApplicationBuilder (optional).--------

app.Run();
```

- **Modules with description the adding process.**
```C#
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
    /// 5. При необходимости создать фильтр авторизации для дашборда Hangfire, унаследованный от <see cref="IDashboardAuthorizationFilter"/> или же использовать существующие фильтры (<see cref="AllAuthorizationFilter"/>).
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
    /// Модуль контроллеров.
    /// </summary>
    Controllers = 7,

    /// <summary>
    /// Модуль Serilog.
    /// 
    /// Настроить модуль можно в конфиге appsettings.json. Базовый конфиг для настроек Serilog можно проинициализировать, используя метод .InitBaseConfig() у WebApplicationFacade.
    /// Обратиться к логгеру можно как используя интерфейс <see cref="ILogger{TCategoryName}"/>, так и используя статический класс <see cref="Serilog.Log"/>.
    /// </summary>
    Serilog = 8,

    /// <summary>
    /// Модуль переменных окружения.
    /// </summary>
    EnvironmentVariables = 9,

    /// <summary>
    /// Модуль конфигурации ApiExplorer (служба Minimal APIs).
    /// </summary>
    EndpointsApiExplorer = 10,

    /// <summary>
    /// Модуль инициализации мигратора <see cref="Migrator"/> (применяет созданные миграции к БД) и проведения миграций при старте приложения с использованием <see cref="MigrationHostedService"/>.
    ///
    /// Для корректной работы мигратора необходимо:
    /// 1. Создать модели <see cref="DbContext"/>.
    /// 2. Присвоить моделям <see cref="DbContext"/> атрибут <see cref="AutoRegisterDbContextAttribute"/>.
    /// 3. Создать миграции, используя команду <code>dotnet ef migrations add InitialCreate --project your-project/your-project.csproj --startup-project your-project/your-project.csproj --output-dir Migrations</code>.
    /// </summary>
    Migrations = 11,

    /// <summary>
    /// Модуль для автоматической регистрации сервисов.
    /// 
    /// Для корректной работы данного модуля необходимо:
    /// 1. Создать сервис и присвоить ему атрибут <see cref="AutoRegisterServiceAttribute"/>.
    /// </summary>
    /// <remarks>
    /// При необходимости добавления фабричной функции во время регистрации сервиса, необходимо унаследовать созданный сервис от <see cref="ServiceBase"/>,
    /// и переопределить в созданном сервисе метод <see cref="ServiceBase.ImplementationFactory"/>.
    /// необходимо
    /// </remarks>
    Services = 12
}
```

- **An example of adding HealthCheck. Just add a class, inherit it from the interface, assign an attribute to it, and create a HealthChecksSettings model in appsettings.json. Reflection will do everything by itself. No more code is needed!**

*Program.cs*
```C#
var modules = new HashSet<WebApplicationModules> 
{
    WebApplicationModules.HealthChecks
};

var builder = new WebApplicationFacade(modules)
    .InitBaseConfig(); // Optional. This can be used if in appsettings.json the HealthCheksSettings model is not initialized manually.

var app = builder.CreateApplication();

app.Run();
```

*RandomHealthCheck.cs*
```C#
[AutoRegisterHealthCheck]
public class RandomHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var random = new Random();
        int number = random.Next(0, 2);

        switch (number)
        {
            case 0:
                return Task.FromResult(HealthCheckResult.Degraded());
            case 1:
                return Task.FromResult(HealthCheckResult.Unhealthy());
            default:
                return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
```
*appsettings.json*
```JSON
{
  ...
  "HealthChecksSettings": {
    "Endpoint": "/health",
    "UIEnable": true,
    "EndpointFullUrl": "https://localhost:44300/health",
    "UIEvaluationTimeInSeconds": 5,
    "UIApiMaxActiveRequests": 2
  }
  ...
}
```

- **Example of registering Hangfire background tasks.**

*Program.cs*
```C#
var modules = new HashSet<WebApplicationModules> 
{
    WebApplicationModules.Hangfire
};

var builder = new WebApplicationFacade(modules)
    .InitBaseConfig(); // Optional. This can be used if in appsettings.json the DateTimeTaskSettings model is not initialized manually.

var app = builder.CreateApplication();

app.Run();
```

*DateTimeService.cs*
```C#
[AutoRegisterService(ServiceLifetime.Transient)]
public class DateTimeService
{
    public async Task<string> GetDateTimeWithMessageAsync()
    {
        var delay = 100;

        await Task.Delay(delay);

        return $"[Delay - {delay}][Current Time] : {DateTime.Now}";
    }
}
```

*DateTimeTaskSettings.cs*
```C#
[AutoRegisterConfigSettings]
public class DateTimeTaskSettings : HangfireTaskSettingsBase
{
}
```

*DateTimeTask.cs*
```C#
[AutoRegisterHangfireTask(typeof(DateTimeTaskSettings))]
public class DateTimeTask : IHangfireBackgroundTask
{
    private readonly DateTimeService _dateTimeService;
    private readonly ILogger<DateTimeTask> _logger;

    public DateTimeTask(
        DateTimeService dateTimeService,
        ILogger<DateTimeTask> logger)
    {
        _dateTimeService = dateTimeService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var currentDateTime = await _dateTimeService.GetDateTimeWithMessageAsync();

        _logger.LogInformation(currentDateTime);
    }
}
```

*appsettings.json*
```C#
{
  ...
  "DateTimeTaskSettings": {
    "CronExpression": "*/5 * * * *",
    "TimeZone": "Europe/Moscow"
  }
  ...
}
```

- **Example of creating docker files - docker-compose.yml and develop.env.**

*Program.cs*
```C#
var modules = new HashSet<WebApplicationModules> 
{
    WebApplicationModules.Settings
};

var dockerComposeModules = new List<DockerComposeFileModules>
{
    DockerComposeFileModules.Server
};

var builder = new WebApplicationFacade(modules)
    .InitBaseConfig()
    .InitBaseDockerComposeFiles(dockerComposeModules); // After launching the application, the files will be created in the application directory. bin/Debug/net8.0/DockerTemplates

var app = builder.CreateApplication();

app.Run();
```

*docker-compose.yml*
```yml
version: "3.9"

networks:
  app-network:

services:

  server:
    build:
      # Можно либо обратиться к конкретному image, либо к Dockerfile.
      #image:
      #context: 
      #dockerfile: 
    container_name: server
    environment:
      TZ: ${TIME_ZONE}
    ports:
      - "8000:8000"
    env_file: ${ENV_FILE}
    networks:
      - app-network
```

*develop.env*
```yml
# Путь до .env файла относительно docker-compose (используется для прописывания .env файлов в сервисы docker-compose).
ENV_FILE=develop.env

# Временная зона приложений.
TIME_ZONE=Europe/Moscow

CorsSettings__EnableCors=True
CorsSettings__PolicyName=AllowAccessFrontendQueries
CorsSettings__AllowedOrigins__0=

HangfireDashboardSettings__EnableCustomAuthorization=True
HangfireDashboardSettings__FilterName=AllAuthorizationFilter

HangfireSettings__StorageType=2
HangfireSettings__PostgreSqlConnectionString=null

HealthChecksSettings__Endpoint=/health
HealthChecksSettings__UIEnable=True
HealthChecksSettings__EndpointFullUrl=https://localhost:44300/health
HealthChecksSettings__UIEvaluationTimeInSeconds=5
HealthChecksSettings__UIApiMaxActiveRequests=2

Serilog__Using__0=Serilog.Sinks.Console
Serilog__Using__1=Serilog.Sinks.SQLite
Serilog__MinimumLevel__Default=Information
Serilog__MinimumLevel__Override__Microsoft=Warning
Serilog__MinimumLevel__Override__System=Warning
Serilog__MinimumLevel__Override__HealthChecks=Warning
Serilog__MinimumLevel__Override__AspNetCore.HealthChecks.UI=Warning
Serilog__MinimumLevel__Override__AspNetCore.HealthChecks.UI.Client=Warning
Serilog__MinimumLevel__Override__AspNetCore.HealthChecks.UI.InMemory.Storage=Warning
Serilog__WriteTo__0__Name=Console
Serilog__WriteTo__0__OutputTemplate=[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}
Serilog__WriteTo__0__Args=null
Serilog__WriteTo__1__Name=SQLite
Serilog__WriteTo__1__OutputTemplate=null
Serilog__WriteTo__1__Args__SqliteDbPath=logs/logs.db
Serilog__WriteTo__1__Args__TableName=Logs
Serilog__Properties__ApplicationName=ApplicationName

DummySettings__DummyText=DummyTextTest
DummySettings__DummyFlag=False
DummySettings__DummyNumber=55,555
DummySettings__DummyArray__0=

DateTimeTaskSettings__CronExpression=*/5 * * * *
DateTimeTaskSettings__TimeZone=Europe/Moscow

DummyDbContextSettings__ConnectionString=Host=localhost:5432; Database=postgres; Username=postgres; Password=admin
DummyDbContextSettings__Schema=DummySchema
DummyDbContextSettings__MigrationsTableName=__EFMigrationsHistory
DummyDbContextSettings__MigrationsSchema=DummySchema
```

*appsettings.json*
```yml
{
  "CorsSettings": {
    "EnableCors": true,
    "PolicyName": "AllowAccessFrontendQueries",
    "AllowedOrigins": []
  },
  "HangfireDashboardSettings": {
    "EnableCustomAuthorization": true,
    "FilterName": "AllAuthorizationFilter"
  },
  "HangfireSettings": {
    "StorageType": 2,
    "PostgreSqlConnectionString": null
  },
  "HealthChecksSettings": {
    "Endpoint": "/health",
    "UIEnable": true,
    "EndpointFullUrl": "https://localhost:44300/health",
    "UIEvaluationTimeInSeconds": 5,
    "UIApiMaxActiveRequests": 2
  },
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console",
      "Serilog.Sinks.SQLite"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning",
        "HealthChecks": "Warning",
        "AspNetCore.HealthChecks.UI": "Warning",
        "AspNetCore.HealthChecks.UI.Client": "Warning",
        "AspNetCore.HealthChecks.UI.InMemory.Storage": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "OutputTemplate": "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        "Args": null
      },
      {
        "Name": "SQLite",
        "OutputTemplate": null,
        "Args": {
          "SqliteDbPath": "logs/logs.db",
          "TableName": "Logs"
        }
      }
    ],
    "Properties": {
      "ApplicationName": "ApplicationName"
    }
  },
  "DummySettings": {
    "DummyText": "DummyTextTest",
    "DummyFlag": false,
    "DummyNumber": 55.555,
    "DummyArray": []
  },
  "DateTimeTaskSettings": {
    "CronExpression": "*/5 * * * *",
    "TimeZone": "Europe/Moscow"
  },
  "DummyDbContextSettings": {
    "ConnectionString": "Host=localhost:5432; Database=postgres; Username=postgres; Password=admin",
    "Schema": "DummySchema",
    "MigrationsTableName": "__EFMigrationsHistory",
    "MigrationsSchema": "DummySchema"
  }
}
```

## Tech Stack
- **.NET 8**

## NuGet Packages
- ```AspNetCore.HealthChecks.UI```
- ```AspNetCore.HealthChecks.UI.Client```
- ```AspNetCore.HealthChecks.UI.InMemory.Storage```
- ```AutoMapper```
- ```EFCore.NamingConventions```
- ```Hangfire.AspNetCore```
- ```Hangfire.Core```
- ```Hangfire.MemoryStorage```
- ```Hangfire.PostgreSql```
- ```Microsoft.EntityFrameworkCore```
- ```Microsoft.EntityFrameworkCore.Relational```
- ```Microsoft.EntityFrameworkCore.Tools```
- ```Microsoft.Extensions.Configuration```
- ```Microsoft.Extensions.DependencyInjection```
- ```Microsoft.Extensions.Hosting```
- ```Microsoft.Extensions.Logging```
- ```Microsoft.Extensions.Options```
- ```Microsoft.Extensions.Options.ConfigurationExtensions```
- ```Newtonsoft.Json```
- ```Npgsql.EntityFrameworkCore.PostgreSQL```
- ```Serilog.AspNetCore```
- ```Serilog.Sinks.SQLite```
- ```Swashbuckle.AspNetCore```

## Authors
- [Pavel Roslyakov](https://github.com/qpashkaaa)

## Contacts
- [Portfolio Website](https://portfolio-website-qpashkaaa.vercel.app/)
- [Telegram](https://t.me/qpashkaaa)
- [VK](https://vk.com/qpashkaaa)
- [LinkedIN](https://www.linkedin.com/in/pavel-roslyakov-7b303928b/)

## Year of Development
> *2024*
  
