using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AspNetCoreMicroserviceInitializer.Registrations.Models;

/// <summary>
/// Модель для хранения методов конфигурации модулей <see cref="WebApplicationModules"/>.
/// </summary>
internal class ConfigureActions
{
    /// <summary>
    /// Метод конфигурации модуля <see cref="WebApplicationModules.Serilog"/>.
    /// </summary>
    public Action<HostBuilderContext, IServiceProvider, LoggerConfiguration>? Serilog { get; set; }
}
