using System.Reflection;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMicroserviceInitializer.Registrations.Extensions;

/// <summary>
/// Расширение для автоматической регистрации AutoMappers.
/// Автоматически регистрирует настройки конфига, у классов которых есть атрибут <see cref="AutoRegisterProfileAttribute"/>.
/// </summary>
internal static class AutoMapperRegistrationExtensions
{
    /// <summary>
    /// Метод добавления мапперов для моделей Domain и DTO.
    /// Автоматически регистрирует настройки конфига, у классов которых есть атрибут <see cref="AutoRegisterProfileAttribute"/>.
    /// </summary>
    internal static IHostApplicationBuilder AddAutoMappers(
        this IHostApplicationBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider();

        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<AutoRegisterProfileAttribute>(false, serviceProvider);
        
        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<AutoRegisterProfileAttribute>(false).Any()),
            type =>
            {
                if (!type.IsSubclassOf(typeof(Profile)))
                {
                    throw new Exception($"Невозможно применить атрибут {nameof(AutoRegisterProfileAttribute)} к модели типа {type.FullName}. Атрибут {nameof(AutoRegisterProfileAttribute)} может применяться только к профилям, которые наследуют {typeof(Profile)}");
                }

                builder.Services.AddAutoMapper(type);
            });

        return builder;
    }
}