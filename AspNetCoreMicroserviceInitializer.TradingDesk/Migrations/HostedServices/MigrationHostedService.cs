using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Migrations.HostedServices;

/// <summary>
/// Сервис для осуществелия миграций БД.
/// </summary>
public class MigrationHostedService : IHostedService
{
    /// <summary>
    /// Мигратор.
    /// </summary>
    private readonly IMigrator _migrator;

    /// <summary>
    /// Интерфейс для работы с жизненным циклом приложения.
    /// </summary>
    private readonly IHostApplicationLifetime _hostApplicationLifetime;

    /// <summary>
    /// Модель настроек мигратора.
    /// </summary>
    private readonly MigratorSettings _migratorSettings;

    /// <summary>
    /// Создание <see cref="MigrationHostedService"/>.
    /// </summary>
    /// <param name="migrator">Мигратор типа <see cref="IMigrator"/>.</param>
    /// <param name="hostApplicationLifetime">Интерфейс для работы с жизненным циклом приложения.</param>
    public MigrationHostedService(
        IMigrator migrator,
        IHostApplicationLifetime hostApplicationLifetime,
        IOptions<MigratorSettings> migratorSettings)
    {
        _migrator = migrator;
        _hostApplicationLifetime = hostApplicationLifetime;
        _migratorSettings = migratorSettings.Value;
    }

    /// <summary>
    /// Метод старта работы.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_migratorSettings.IsStopApplicationAfterApplyMigrations)
        {
            _hostApplicationLifetime.ApplicationStarted.Register(o => _hostApplicationLifetime.StopApplication(), null);
        }

        await _migrator.MigrateAsync(cancellationToken);
    }

    /// <summary>
    /// Метод остановки работы.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
    }
}