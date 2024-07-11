using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

/// <summary>
/// Атрибут для автоматической регистрации <see cref="IHealthCheck"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterHealthCheckAttribute : Attribute
{
}
