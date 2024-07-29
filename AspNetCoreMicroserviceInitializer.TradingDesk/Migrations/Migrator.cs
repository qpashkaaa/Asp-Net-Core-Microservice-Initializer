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
    private readonly IServiceScopeFactory _scopeFactory;

    /// <summary>
    /// Создание <see cref="Migrator"/>.
    /// </summary>
    public Migrator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Метод выполнения миграции.
    /// </summary>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContexts = new List<DbContext>();

            var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterDbContextAttribute>(false, scope.ServiceProvider as ServiceProvider);

            AssemblyHelper.FindTypesByConditionAndDoActions(
                assemblies,
                assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<AutoRegisterDbContextAttribute>(false).Any()),
                type =>
                {
                    if (!type.IsSubclassOf(typeof(DbContext)))
                    {
                        throw new Exception($"Невозможно применить атрибут {nameof(AutoRegisterDbContextAttribute)} к модели типа {type.FullName}. Атрибут {nameof(AutoRegisterDbContextAttribute)} может применяться только к профилям, которые наследуют {typeof(DbContext)}");
                    }

                    if (scope.ServiceProvider.GetRequiredService(type) is DbContext dbContext)
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
}