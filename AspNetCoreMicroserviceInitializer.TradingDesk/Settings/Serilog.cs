using System.Text.Json.Serialization;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Settings;

/// <summary>
/// Базовая модель настроек Serilog.
/// </summary>
/// <remarks>Используется только для базовой инициализации элемента в конфиге, если его там нет.</remarks>
[AutoRegisterConfigSettings]
[ConfigSettingsModule(WebApplicationModules.Serilog)]
class Serilog
{
    /// <summary>
    /// Используемые модули.
    /// </summary>
    string[ ] Using { get; } = new[ ] { "Serilog.Sinks.Console", "Serilog.Sinks.SQLite" };

    /// <summary>
    /// Модель минимального уровня логирования.
    /// </summary>
    MinimumLevelElement MinimumLevel { get; } = new MinimumLevelElement();

    /// <summary>
    /// Массив элементов, куда будут записываться сообщения.
    /// </summary>
    WriteToElement[ ] WriteTo { get; } = new[ ]
    {
        new WriteToElement()
        {
            Name = "Console",
            OutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
        },
        new WriteToElement()
        {
            Name = "SQLite",
            Args = new ArgsElement()
            {
                SqliteDbPath = "logs/logs.db",
                TableName = "Logs"
            }
        }
    };

    /// <summary>
    /// Модель свойств.
    /// </summary>
    PropertiesElement Properties { get; } = new PropertiesElement();
}

/// <summary>
/// Модель минимального уровня логирования.
/// </summary>
class MinimumLevelElement
{
    /// <summary>
    /// Модель логирования уровня "Default".
    /// </summary>
    string Default { get; } = "Information";

    /// <summary>
    /// Модель "перезаписи" уровней логирования.
    /// </summary>
    OverrideElement Override { get; } = new OverrideElement();
}

/// <summary>
/// Модель "перезаписи" уровней логирования.
/// </summary>
class OverrideElement
{
    /// <summary>
    /// Уровень логирования "Microsoft".
    /// </summary>
    string Microsoft { get; } = "Warning";

    /// <summary>
    /// Уровень логирования "System".
    /// </summary>
    string System { get; } = "Warning";

    /// <summary>
    /// Уровень логирования "HealthChecks".
    /// </summary>
    string HealthChecks { get; } = "Warning";

    /// <summary>
    /// Уровень логирования "AspNetCore.HealthChecks.UI".
    /// </summary>
    [JsonPropertyName("AspNetCore.HealthChecks.UI")]
    string AspNetCoreHealthChecksUI { get; } = "Warning";

    /// <summary>
    /// Уровень логирования "AspNetCore.HealthChecks.UI.Client".
    /// </summary>
    [JsonPropertyName("AspNetCore.HealthChecks.UI.Client")]
    string AspNetCoreHealthChecksUIClient { get; } = "Warning";

    /// <summary>
    /// Уровень логирования "AspNetCore.HealthChecks.UI.InMemory.Storage"".
    /// </summary>
    [JsonPropertyName("AspNetCore.HealthChecks.UI.InMemory.Storage")]
    string AspNetCoreHealthChecksUIInMemoryStorage { get; } = "Warning";
}

/// <summary>
/// Элемент способа записи сообщения.
/// </summary>
class WriteToElement
{
    /// <summary>
    /// Имя.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Шаблон вывода информации.
    /// </summary>
    public string? OutputTemplate { get; init; }

    /// <summary>
    /// Аргументы.
    /// </summary>
    public ArgsElement? Args { get; init; }
}

/// <summary>
/// Модель аргументов.
/// </summary>
class ArgsElement
{
    /// <summary>
    /// Путь до базы Sqlite.
    /// </summary>
    public string? SqliteDbPath { get; init; }

    /// <summary>
    /// Название таблицы.
    /// </summary>
    public string? TableName { get; init; }
}

/// <summary>
/// Модель свойств.
/// </summary>
class PropertiesElement
{
    /// <summary>
    /// Название приложения.
    /// </summary>
    string ApplicationName { get; } = "ApplicationName";
}