using AspNetCoreMicroserviceInitializer.Registrations.Models;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes.Abstract;
using AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AspNetCoreMicroserviceInitializer.Registrations.Helpers;

/// <summary>
/// Хелпер для регистрации сервисов.
/// </summary>
internal static class ServicesRegistrationHelper
{
    /// <summary>
    /// Метод регистрации сервиса.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    /// <param name="serviceInfo">Модель с информацией о сервисе.</param>
    public static void RegisterService(
        IServiceCollection services,
        ServiceTypeInfo serviceInfo)
    {
        var methodName = $"Add{serviceInfo.Lifetime}";
        var parametersLenth = serviceInfo.ImplementationFactory == null ? 1 : 2;

        var genericTypes = new Type?[ ]
        {
            serviceInfo.InterfaceType,
            serviceInfo.ServiceType
        }
        .Where(type => type != null)
        .Cast<Type>()
        .ToArray();

        var addMethod = typeof(ServiceCollectionServiceExtensions)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .FirstOrDefault(method => method.Name == methodName &&
                                      method.IsGenericMethod &&
                                      method.GetGenericArguments().Length == genericTypes.Length &&
                                      method.GetParameters().Length == parametersLenth &&
                                      (parametersLenth == 1 || method.GetParameters()[1].ParameterType.IsGenericType &&
                                                               method.GetParameters()[1].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>)
                                      ))?
            .MakeGenericMethod(genericTypes);

        if (addMethod != null)
        {
            var parameters = new List<object> { services };

            if (serviceInfo.ImplementationFactory != null)
            {
                parameters.Add((object)serviceInfo.ImplementationFactory);
            }

            addMethod.Invoke(null, parameters.ToArray());
        }
    }

    /// <summary>
    /// Метод получения фабричной функции для создания экземпляра сервиса внутри Dependency Injection контейнера.
    /// </summary>
    /// <param name="type">Тип сервиса.</param>
    /// <returns>
    /// <see langword="null"/>, если сервис не реализует интерфейс <see cref="IServiceImplementationFactory{TService}"/>,
    /// <see cref="Func{IServiceProvider, object}"/>, если у сервиса реализован интерфейс <see cref="IServiceImplementationFactory{TService}"/> и корректно установлен дженерик-тип.
    /// </returns>
    public static Func<IServiceProvider, object>? GetImplementationFactory(Type type)
    {
        var implementationFactoryInterface = type
            .GetInterfaces()
            .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IServiceImplementationFactory<>))
            .FirstOrDefault();

        if (implementationFactoryInterface != null)
        {
            if (implementationFactoryInterface.GetGenericArguments().First() != type)
            {
                throw new Exception($"Дженерик-тип интерфейса {typeof(IServiceImplementationFactory<>)} должен быть идентичен типу класса {type}, в котором реализуется данный интерфейс.");
            }

            var instance = Activator.CreateInstance(type);

            if (instance == null)
            {
                throw new Exception($"Не удалось создать экземпляр типа {type.Name}");
            }

            var implementationFactoryProperty = type.GetProperty(nameof(IServiceImplementationFactory<object>.ImplementationFactory));

            if (implementationFactoryProperty != null)
            {
                return (Func<IServiceProvider, object>?)implementationFactoryProperty.GetValue(instance);
            }
        }

        return null;
    }

    /// <summary>
    /// Метод получения отсортированного массива (<see cref="IOrderedEnumerable{ServiceTypeInfo}"/>) сервисов, которые требуют регистрации, по количеству параметров конструктора (от меньшего к большему).
    /// </summary>
    /// <typeparam name="TAttribute">Тип атрибута, по которому нужно искать сервисы.</typeparam>
    /// <param name="services">Коллекция сервисов.</param>
    /// <returns>Отсортированный массив <see cref="IOrderedEnumerable{ServiceTypeInfo}"/> сервисов, которые требуют регистрацию.</returns>
    public static IOrderedEnumerable<ServiceTypeInfo> GetOrderedServicesTypes<TAttribute>(IServiceCollection services)
        where TAttribute : AutoRegisterServiceBaseAttribute
    {
        var servicesInfo = new List<ServiceTypeInfo>();

        var serviceProvider = services.BuildServiceProvider();

        var assemblies = AssemblyHelper.LoadAssembliesWithSpecificAttribute<TAttribute>(false, serviceProvider).ToList();

        AssemblyHelper.FindTypesByConditionAndDoActions(
            assemblies,
            assembly => assembly.GetTypes().Where(t => t.GetCustomAttributes<TAttribute>(false).Any()),
            type =>
            {
                var attribute = type.GetCustomAttribute<TAttribute>() ?? throw new Exception($"У модели {type.Name} не указан атрибут {typeof(TAttribute).Name} для автоматической регистрации сервиса.");

                var serviceInfo = new ServiceTypeInfo
                {
                    ServiceType = type,
                    ConstructorsParamsCount = type.GetConstructors().Sum(x => x.GetParameters().Length),
                    Lifetime = attribute.ServiceLifetime,
                    InterfaceType = attribute.IntefaceType,
                    ImplementationFactory = ServicesRegistrationHelper.GetImplementationFactory(type)
                };

                servicesInfo.Add(serviceInfo);
            });

        return servicesInfo.OrderBy(x => x.ConstructorsParamsCount);
    }
}
