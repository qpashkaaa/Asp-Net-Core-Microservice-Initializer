using System.Reflection;
using AspNetCoreMicroserviceInitializer.Registrations.Extensions;
using Microsoft.AspNetCore.Builder;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using Microsoft.Extensions.Hosting;
using AspNetCoreMicroserviceInitializer.Registrations.Models;

namespace AspNetCoreMicroserviceInitializer.Registrations.Builders;

/// <summary>
/// Фасад для настройки <see cref="WebApplication"/> и <see cref="WebApplicationBuilder"/>.
/// </summary>
public class WebApplicationFacade
{
    /// <summary>
    /// Модули веб-приложения.
    /// </summary>
    private HashSet<WebApplicationModules> _modules;
    
    /// <summary>
    /// Билдер веб-приложения.
    /// </summary>
    private readonly WebApplicationBuilder _builder;

    /// <summary>
    /// Модель для хранения методов конфигурации модулей <see cref="WebApplicationModules"/>.
    /// </summary>
    private readonly ConfigureActions _configureActions;

    /// <summary>
    /// Конструктор фасада для настройки <see cref="WebApplication"/> и <see cref="WebApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder">Преднастроенный билдер.</param>
    /// <param name="modules">Модули веб-приложения.</param>
    public WebApplicationFacade(
        HashSet<WebApplicationModules> modules, 
        WebApplicationBuilder? builder = null)
    {
        _builder = builder ?? WebApplication.CreateBuilder();
        _modules = modules;
        _configureActions = new ConfigureActions();
    }
    
    /// <summary>
    /// Метод добавления доп. модулей в билдер (регистрация сервисов и т.д.).
    /// </summary>
    /// <param name="addModulesAction">Action добавления модулей.</param>
    public WebApplicationFacade AddAdditionalModules(Action<WebApplicationBuilder> addModulesAction)
    {
        addModulesAction(_builder);

        return this;
    }

    /// <summary>
    /// Метод добавления доп. конфигурации Serilog.
    /// </summary>
    /// <param name="configureLogger">Метод конфигурации логгера.</param>
    /// <remarks>Метод чтения из конфигурации и сервисов изначально добавлен в конфигурации Serilog. Добавлять его, используя этот метод не нужно.</remarks>
    public WebApplicationFacade AddAdditionalSerilogConfiguration(Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> configureLogger)
    {
        _configureActions.Serilog = configureLogger;

        return this;
    }

    /// <summary>
    /// Метод инициализации стандартного конфига appsettings.json значениями по умолчанию, используя модели настроек.
    /// </summary>
    /// <remarks>
    /// При использовании данного метода, в конфиг добавятся секции моделей настроек со значениями по умолчанию, если этой секции не существует.
    /// У моделей настроек должен быть атрибут <see cref="AutoRegisterConfigSettingsAttribute"/>.
    /// </remarks>
    /// <param name="configPath">Путь до файла конфига.</param>
    public WebApplicationFacade InitBaseConfig(string configPath = "appsettings.json")
    {
        var serviceProvider = _builder.Services.BuildServiceProvider();

        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterConfigSettingsAttribute>(false, serviceProvider);
        
        var json = File.ReadAllText(configPath);
        var jObject = JObject.Parse(json);
        
        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<AutoRegisterConfigSettingsAttribute>(false).Any()),
            type =>
            {
                Func<Attribute, object?> attributeGetValueFunc = attr => attr.GetType()?.GetField("WebApplicationModule")?.GetValue(attr);

                var typeModule = (WebApplicationModules?)type
                    .GetCustomAttributes()
                    .Where(attr => attributeGetValueFunc(attr) != null)
                    .Select(attr => attributeGetValueFunc(attr))
                    .FirstOrDefault();

                if (typeModule != null && 
                    !_modules.Contains(typeModule.Value))
                {
                    return;
                }

                var modelObject = JsonHelper.GetModelJObject(type);

                if (!jObject.ContainsKey(type.Name))
                {
                    jObject[type.Name] = modelObject;
                }
                else
                {
                    var existingObject = jObject[type.Name] as JObject;
                    JsonHelper.AddMissingProperties(existingObject ?? throw new ArgumentNullException(nameof(existingObject)), modelObject);
                }
            });
        
        File.WriteAllText(configPath, jObject.ToString(Formatting.Indented));
        File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configPath), jObject.ToString(Formatting.Indented));

        return this;
    }

    /// <summary>
    /// Метод инициализации стандартных файлов docker-compose, используя конфиг appsettings.json.
    ///
    /// Метод создает файлы:
    /// 1) develop.env
    /// 2) docker-compose.yml
    /// </summary>
    /// <remarks>Файлы создаются в директории приложения в папке Templates. После их можно вынести, куда необходимо.</remarks>
    /// <param name="filesModules">Модули, которые будут добавлены в файлы.</param>
    /// <param name="configPath">Путь до файла конфига.</param>
    public WebApplicationFacade InitBaseDockerComposeFiles(
        List<DockerComposeFileModules> dockerComposeFileModules,
        string configPath = "appsettings.json")
    {
        DockerComposeFilesHelper.CreateDockerComposeFileContent(dockerComposeFileModules);
        DockerComposeFilesHelper.CreateEnvironmentFileContent(configPath);
        
        return this;
    }

    /// <summary>
    /// Метод создания веб-приложения с подключенными модулями в билдере.
    /// </summary>
    /// <returns>Веб-приложение типа <see cref="WebApplication"/> с установленными зависимостями.</returns>
    public WebApplication CreateApplication()
    {
        AddInitializedModules();

        var app = _builder.Build();

        foreach (var module in _modules)
        {
            switch (module)
            {
                case WebApplicationModules.Cors:
                    app.UseNamedPolicyCors();
                    break;
                case WebApplicationModules.Hangfire:
                    app.UseHangfireDashboard();
                    app.UseHangfireTasks();
                    break;
                case WebApplicationModules.HealthChecks:
                    app.UseHealthChecks();
                    break;
                case WebApplicationModules.Swagger:
                    app.UseSwagger();
                    app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
                    });
                    break;
                case WebApplicationModules.Authorization:
                    app.UseAuthorization();
                    break;
                case WebApplicationModules.Controllers:
                    app.MapControllers();
                    app.UseRouting();
                    break;
                default:
                    continue;

            }
        }

        return app;
    }

    /// <summary>
    /// Метод добавления проинициализированных модулей в билдер <see cref="WebApplicationBuilder"/>.
    /// </summary>
    private void AddInitializedModules()
    {
        foreach (var module in _modules)
        {
            switch (module)
            {
                case WebApplicationModules.AutoMappers:
                    _builder.AddAutoMappers();
                    break;
                case WebApplicationModules.Cors:
                    _builder.AddNamedPolicyCors();
                    break;
                case WebApplicationModules.Hangfire:
                    _builder.Services.AddHangfireTasks();
                    _builder.AddHangfire();
                    break;
                case WebApplicationModules.HealthChecks:
                    _builder.Services.AddHealthChecks(_builder.Configuration);
                    break;
                case WebApplicationModules.Settings:
                    _builder.Services.AddSettings(_builder.Configuration);
                    break;
                case WebApplicationModules.Database:
                    _builder.Services.AddDbServices();
                    break;
                case WebApplicationModules.Swagger:
                    _builder.Services.AddSwaggerGen(c =>
                    {
                        c.SwaggerDoc("Documentation", new OpenApiInfo { Title = "API Documentation", Version = "latest" });
                    });
                    break;
                case WebApplicationModules.Controllers:
                    _builder.Services.AddControllers();
                    break;
                case WebApplicationModules.Serilog:
                    _builder.Host.UseSerilog((context, services, configuration) =>
                    {
                        configuration
                            .ReadFrom.Configuration(context.Configuration)
                            .ReadFrom.Services(services);

                        if (_configureActions.Serilog != null)
                        {
                            _configureActions.Serilog(context, services, configuration);
                        }
                    });
                    break;
                case WebApplicationModules.EnvironmentVariables:
                    _builder.Configuration.AddEnvironmentVariables();
                    break;
                case WebApplicationModules.EndpointsApiExplorer:
                    _builder.Services.AddEndpointsApiExplorer();
                    break;
                case WebApplicationModules.Migrations:
                    _builder.AddMigrator();
                    break;
                default:
                    continue;
            }
        }
    }
}