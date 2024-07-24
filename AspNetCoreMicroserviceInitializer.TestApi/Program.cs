using AspNetCoreMicroserviceInitializer.Registrations.Builders;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;

namespace AspNetCoreMicroserviceInitializer.TestApi;

public class Program
{
    public static void Main(string[ ] args)
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
        //    WebApplicationModules.Swagger
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

        app.Run();
    }
}
