using System.Reflection;
using AspNetCoreMicroserviceInitializer.TradingDesk.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;

/// <summary>
/// Класс хелперов для работы с <see cref="Assembly"/>.
/// </summary>
public static class AssemblyHelper
{
    /// <summary>
    /// Загрузить сборки в которых присутствуют классы с определенным типом.
    /// </summary>
    /// <typeparam name="T">Тип который должен находиться в сборке.</typeparam>
    /// <param name="assembliesToLoadReferences">Сборки ссылки которых нужно дополнительно загрузить, чтобы избежать появления лишних сборок (необязательный параметр).</param>
    /// <param name="serviceProvider"><see cref="ServiceProvider"/>. Нужен, чтобы из DI получить логгер и записать ошибку в случае, когда не получается загрузить <see cref="Assembly"/> (необязательный параметр).</param>
    /// <returns>Подходящие сборки типа <see cref="Assembly"/>.</returns>
    public static IEnumerable<Assembly> LoadAssembliesWithSpecificType<T>(
        ServiceProvider? serviceProvider = null)
    {
        var assembliesWithSpecificType = LoadAssembliesWithSpecificConditionFunction(assembly =>
        {
            return assembly.GetTypes().Any(type => type.IsClass &&
                                                   !type.IsAbstract &&
                                                   !type.IsInterface &&
                                                   typeof(T).IsAssignableFrom(type));
        }, serviceProvider);

        return assembliesWithSpecificType;
    }
    
    /// <summary>
    /// Загрузить сборки в которых присутствуют классы с определенным атрибутом.
    /// </summary>
    /// <typeparam name="TAttribute">Тип атрибута который должен находиться в сборке.</typeparam>
    /// <param name="assembliesToLoadReferences">Сборки ссылки которых нужно дополнительно загрузить, чтобы избежать появления лишних сборок (необязательный параметр).</param>
    /// <param name="serviceProvider"><see cref="ServiceProvider"/>. Нужен, чтобы из DI получить логгер и записать ошибку в случае, когда не получается загрузить <see cref="Assembly"/> (необязательный параметр).</param>
    /// <returns>Подходящие сборки типа <see cref="Assembly"/>.</returns>
    public static IEnumerable<Assembly> LoadAssembliesWithSpecificAttribute<TAttribute>(
        bool inherit,
        ServiceProvider? serviceProvider = null) where TAttribute : Attribute
    {
        var assembliesWithSpecificType = LoadAssembliesWithSpecificConditionFunction(assembly =>
        {
            return assembly.GetTypes().Any(type => 
                type.GetCustomAttributes<TAttribute>(inherit: true).Any() || 
                type.GetMembers().OfType<MemberInfo>().Any(member => member.GetCustomAttributes<TAttribute>(inherit).Any()));
        }, serviceProvider);

        return assembliesWithSpecificType;
    }
    
    /// <summary>
    /// Загрузить сборки с определенной функцией условия (функция, при которой сборка должна быть загружена).
    /// </summary>
    /// <param name="conditionFunction">Функция условия загрузки сборки.</param>
    /// <param name="serviceProvider"><see cref="ServiceProvider"/>. Нужен, чтобы из DI получить логгер и записать ошибку в случае, когда не получается загрузить <see cref="Assembly"/> (необязательный параметр).</param>
    /// <returns>Подходящие сборки типа <see cref="Assembly"/>.</returns>
    public static IEnumerable<Assembly> LoadAssembliesWithSpecificConditionFunction(
        Func<Assembly, bool> conditionFunction,
        ServiceProvider? serviceProvider = null)
    {
        var loggerFactory = serviceProvider?.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory?.CreateLogger(typeof(AssemblyHelper));
        
        var basePath = AppDomain.CurrentDomain.BaseDirectory;
        var allAssemblyFiles = Directory.GetFiles(basePath, "*.dll");
        var assembliesWithSpecificType = new List<Assembly>();

        foreach (var assemblyPath in allAssemblyFiles)
        {
            try
            {
                var assemblyName = AssemblyName.GetAssemblyName(assemblyPath);

                var assembly = Assembly.Load(assemblyName);

                bool containsSpecificType = conditionFunction(assembly);

                if (containsSpecificType)
                {
                    assembliesWithSpecificType.Add(assembly);
                }
            }
            catch (AttributeException attrEx)
            {
                logger?.LogError(attrEx, "Ошибка с обработкой атрибута при загрузке Assembly.");
                throw attrEx;
            }
            catch (Exception ex)
            {
                logger?.LogInformation(ex, "Не удалось загрузить Assembly.");
            }
        }

        return assembliesWithSpecificType;
    }

    /// <summary>
    /// Найти в сборках типы по условию и выполнить действия с ними (регистрация или использование в методе и т.д.).
    /// <param name="typesConditionFunction">Функция для поиска типов по сборкам.</param>
    /// <param name="typesAction">Действие, которое нужно выполнить с найденными типами.</param>
    /// </summary>
    public static void FindTypesByConditionAndDoActions(
        IEnumerable<Assembly?> assemblies, 
        Func<Assembly, IEnumerable<Type>> typesConditionFunction,
        Action<Type> typesAction)
    {
        foreach (var assembly in assemblies)
        {
            if (assembly != null)
            {
                var types = typesConditionFunction(assembly);

                foreach (var type in types)
                {
                    typesAction(type);
                }
            }
        }
    }

    /// <summary>
    /// Метод загрузки сборок из ссылок.
    /// </summary>
    /// <param name="assembly">Сборка, из которой нужно получить сборки по ссылкам.</param>
    /// <param name="loadedAssemblies">Список загруженных сборок.</param>
    private static void LoadAssembliesFromReferences(
        Assembly assembly,
        List<Assembly> loadedAssemblies)
    {
        var refAssemblyNames = assembly.GetReferencedAssemblies()
            .Where(name => !loadedAssemblies.Any(loadedAssembly => loadedAssembly.FullName == name.FullName)).ToList();

        loadedAssemblies.AddRange(refAssemblyNames.Select(Assembly.Load));
    }
}
