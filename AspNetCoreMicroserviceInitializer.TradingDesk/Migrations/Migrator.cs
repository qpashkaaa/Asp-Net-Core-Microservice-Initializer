using System.Reflection;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Migrations;

/// <summary>
/// Сервис мигратора.
/// </summary>
public class Migrator : IMigrator
{
    /// <summary>
    /// Провайдер сервисов.
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Создание <see cref="Migrator"/>.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public Migrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Метод выполнения миграции.
    /// </summary>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        var dbContexts = new List<DbContext>();

        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterDbContextAttribute>(false, _serviceProvider as ServiceProvider);
        
        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<AutoRegisterDbContextAttribute>(false).Any()),
            type =>
            {
                if (!type.IsSubclassOf(typeof(DbContext)))
                {
                    throw new Exception($"Невозможно применить атрибут {nameof(AutoRegisterDbContextAttribute)} к модели типа {type.FullName}. Атрибут {nameof(AutoRegisterDbContextAttribute)} может применяться только к профилям, которые наследуют {typeof(DbContext)}");
                }

                if (_serviceProvider.GetRequiredService(type) is DbContext dbContext)
                {
                    dbContexts.Add(dbContext);
                }
            });

        foreach (var dbContext in dbContexts)
        {
            await dbContext.Database.MigrateAsync(cancellationToken);
        }
    }
}