using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Migrations;
using AspNetCoreMicroserviceInitializer.TradingDesk.Migrations.HostedServices;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для регистрации зависимостей миграций.
/// </summary>
internal static class MigrationsRegistrationExtensions
{
    /// <summary>
    /// Метод добавления миграций в builder.
    /// </summary>
    internal static IHostApplicationBuilder AddMigrator(
        this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMigrator, Migrator>();
        builder.Services.AddHostedService<MigrationHostedService>();

        return builder;
    }
}