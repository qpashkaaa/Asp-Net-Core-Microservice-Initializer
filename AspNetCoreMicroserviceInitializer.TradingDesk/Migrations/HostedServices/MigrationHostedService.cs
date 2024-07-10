using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;
using Microsoft.Extensions.Hosting;

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
    /// Создание <see cref="MigrationHostedService"/>.
    /// </summary>
    /// <param name="migrator">Мигратор типа <see cref="IMigrator"/>.</param>
    /// <param name="hostApplicationLifetime">Интерфейс для работы с жизненным циклом приложения.</param>
    public MigrationHostedService(IMigrator migrator)
    {
        _migrator = migrator;
    }

    /// <summary>
    /// Метод старта работы.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены.</param>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
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