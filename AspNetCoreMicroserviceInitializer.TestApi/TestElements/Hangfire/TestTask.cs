using System.Diagnostics;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;

namespace AspNetCoreMicroserviceInitializer.TestApi.TestElements.Hangfire;

[AutoRegisterHangfireTask(typeof(TestTaskSettings))]
public class TestTask : IHangfireBackgroundTask
{
    public Task ExecuteAsync()
    {
        Trace.WriteLine("Task works");
        return Task.CompletedTask;
    }
}
