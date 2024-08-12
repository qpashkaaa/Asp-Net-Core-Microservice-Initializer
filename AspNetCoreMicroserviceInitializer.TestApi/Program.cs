//using AspNetCoreMicroserviceInitializer.Registrations.Builders;
//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Database;
//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services;
//using AspNetCoreMicroserviceInitializer.TestApi.TestElements.Services.Interfaces;
//using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;
//using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMicroserviceInitializer.TestApi;

public class Program
{
    public static void Main()
    {
        #region AutoMappers (works)
        //var app = new WebApplicationFacade(
        //    [ 
        //    WebApplicationModules.AutoMappers, 
        //    WebApplicationModules.Controllers, 
        //    WebApplicationModules.Swagger
        //    ])
        //    .CreateApplication();
        #endregion

        #region Cors (works)
        //var app = new WebApplicationFacade(
        //    [
        //    WebApplicationModules.Cors,
        //    WebApplicationModules.Controllers,
        //    WebApplicationModules.Swagger,
        //    WebApplicationModules.EndpointsApiExplorer
        //    ])
        //    .InitBaseConfig()
        //    .CreateApplication();
        #endregion

        #region Hangfire (Works)
        //var app = new WebApplicationFacade(
        //    [
        //    WebApplicationModules.Hangfire,
        //    WebApplicationModules.Settings
        //    ])
        //    .InitBaseConfig()
        //    .CreateApplication();
        #endregion

        #region Health Checks (Works)
        //var app = new WebApplicationFacade(
        //    [
        //    WebApplicationModules.HealthChecks
        //    ])
        //    .InitBaseConfig()
        //    .CreateApplication();
        #endregion

        #region Settings (Works)
        //var app = new WebApplicationFacade(
        //    [
        //        WebApplicationModules.Settings,
        //        WebApplicationModules.Swagger,
        //        WebApplicationModules.Controllers
        //    ])
        //    .InitBaseConfig()
        //    .CreateApplication();
        #endregion

        #region Database (Works)
        //var app = new WebApplicationFacade(
        //    [
        //        WebApplicationModules.Database,
        //        WebApplicationModules.Swagger,
        //        WebApplicationModules.Controllers,
        //        WebApplicationModules.Settings
        //    ])
        //    .InitBaseConfig()
        //    .CreateApplication();
        #endregion

        #region Migrator (Works)
        //var app = new WebApplicationFacade(
        //    [
        //        WebApplicationModules.Migrations
        //    ])
        //    .CreateApplication();
        #endregion

        #region Serilog (Works)
        //var app = new WebApplicationFacade(
        //    [
        //        WebApplicationModules.Serilog,
        //        WebApplicationModules.Controllers,
        //        WebApplicationModules.Swagger
        //    ])
        //    .InitBaseConfig()
        //    .CreateApplication();
        #endregion

        #region EnvironmentVariables (Works)
        //var app = new WebApplicationFacade(
        //    [
        //        WebApplicationModules.Swagger,
        //        WebApplicationModules.EnvironmentVariables,
        //        WebApplicationModules.Settings
        //    ])
        //    .CreateApplication();
        #endregion

        #region Services (Works)
        //var app = new WebApplicationFacade(
        //    [
        //        WebApplicationModules.Services,
        //        WebApplicationModules.Swagger
        //    ])
        //    .CreateApplication();
        #endregion

        //app.Run();
    }
}
