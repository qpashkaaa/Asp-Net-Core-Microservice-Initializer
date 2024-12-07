using AspNetCoreMicroserviceInitializer.Database.Interfaces.MongoDb;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.HealthChecks;

[AutoRegisterHealthCheck]
public class RandomHealthCheck : IHealthCheck
{
    private readonly IMongoClientFactory _test;

    public RandomHealthCheck(IMongoClientFactory test)
    {
        _test = test;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var random = new Random();
        int number = random.Next(0, 2);

        switch (number)
        {
            case 0:
                return Task.FromResult(HealthCheckResult.Degraded());
            case 1:
                return Task.FromResult(HealthCheckResult.Unhealthy());
            default:
                return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
