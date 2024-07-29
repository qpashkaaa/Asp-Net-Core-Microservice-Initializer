using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.HealthChecks;

[AutoRegisterHealthCheck]
public class SecondHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Degraded());
    }
}
