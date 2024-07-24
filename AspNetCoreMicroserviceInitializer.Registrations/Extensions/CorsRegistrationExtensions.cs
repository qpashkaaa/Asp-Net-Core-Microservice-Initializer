using AspNetCoreMicroserviceInitializer.TradingDesk.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для регистрации зависимостей Cors.
/// Модель настроек конфига <see cref="CorsSettings"/>.
/// </summary>
internal static class CorsRegistrationExtensions
{
    /// <summary>
    /// Метод добавления именованной политики Cors с настройками из конфига.
    /// </summary>
    /// <param name="builder">Билдер приложения.</param>
    internal static IHostApplicationBuilder AddNamedPolicyCors(
        this IHostApplicationBuilder builder)
    {
        var corsSettings = builder.Configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();

        if (corsSettings != null &&
            corsSettings.EnableCors &&
            !string.IsNullOrEmpty(corsSettings.PolicyName))
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: corsSettings.PolicyName,
                                  policy =>
                                  {
                                      if (corsSettings.AllowedOrigins != null &&
                                          corsSettings.AllowedOrigins.Length > 0)
                                      {
                                          policy.WithOrigins(corsSettings.AllowedOrigins).AllowAnyHeader().AllowAnyMethod();
                                      }
                                      else
                                      {
                                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                                      }
                                  });
            });
        }

        return builder;
    }

    /// <summary>
    /// Метод подключения именованной политики Cors с настройками из конфига.
    /// </summary>
    /// <param name="app">Веб-приложение, используемое для настройки HTTP-конвейера и маршрутов.</param>
    internal static WebApplication UseNamedPolicyCors(
        this WebApplication app)
    {
        var corsSettings = app.Configuration.GetSection(nameof(CorsSettings)).Get<CorsSettings>();

        if (corsSettings != null &&
            corsSettings.EnableCors &&
            !string.IsNullOrEmpty(corsSettings.PolicyName))
        {
            app.UseCors(corsSettings.PolicyName);
        }

        return app;
    }
}
