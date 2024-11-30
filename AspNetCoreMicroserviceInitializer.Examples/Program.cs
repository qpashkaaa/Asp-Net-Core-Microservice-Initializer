using AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services;
using AspNetCoreMicroserviceInitializer.Registrations.Builders;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;
using Serilog.Filters;
using StackExchange.Redis;

namespace AspNetCoreMicroserviceInitializer.Examples;

public class Program
{
    public static void Main(string[ ] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // --------Creating a WebApplication with all possible parameters and passing a pre-configured WebApplicationBuilder (optional).--------

        var app = new WebApplicationFacade(
        modules: new List<WebApplicationModules>
        {
            WebApplicationModules.AutoMappers,
            WebApplicationModules.Cors,
            WebApplicationModules.Hangfire,
            WebApplicationModules.HealthChecks,
            WebApplicationModules.Settings,
            WebApplicationModules.SqlDatabase,
            WebApplicationModules.Swagger,
            WebApplicationModules.Controllers,
            WebApplicationModules.Serilog,
            WebApplicationModules.EnvironmentVariables,
            WebApplicationModules.EndpointsApiExplorer,
            //WebApplicationModules.EFMigrations,
            WebApplicationModules.Services,
            WebApplicationModules.MongoDatabase,
            WebApplicationModules.RedisDatabase
        },
        builder: builder)
            .AddAdditionalModules(builder =>
            {
                builder.Services.AddTransient<AdditionalGuidService>();
            })
            .AddAdditionalSerilogConfiguration((builder, serviceProvider, configuration) =>
            {
                configuration.Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", path =>
                    "/health".Equals(path, StringComparison.OrdinalIgnoreCase)));
            })
            .InitBaseConfig(configPath: "appsettings.json")
            .InitBaseDockerComposeFiles(
            dockerComposeFileModules: new List<DockerComposeFileModules>
            {
                DockerComposeFileModules.Server,
                DockerComposeFileModules.Client,
                DockerComposeFileModules.Adminer,
                DockerComposeFileModules.MongoDb,
                DockerComposeFileModules.MongoExpress,
                DockerComposeFileModules.ClickHouse,
                DockerComposeFileModules.MySql,
                DockerComposeFileModules.Redis,
                DockerComposeFileModules.Elasticsearch,
                DockerComposeFileModules.Kibana,
                DockerComposeFileModules.Cassandra,
                DockerComposeFileModules.RabbitMq,
                DockerComposeFileModules.Prometheus,
                DockerComposeFileModules.Grafana,
                DockerComposeFileModules.Nginx,
                DockerComposeFileModules.PostgreSql
            },
            configPath: "appsettings.json")
            .CreateApplication();

        // --------Creating a WebApplication with all possible parameters and passing a pre-configured WebApplicationBuilder (optional).--------

        app.Run();
    }
}
