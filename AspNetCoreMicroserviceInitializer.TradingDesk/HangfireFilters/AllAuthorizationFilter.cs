using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.HangfireFilters;

/// <summary>
/// Фильтр для автоматической авторизации всех запросов, при запросе дашборда <see cref="Hangfire"/>.
/// </summary>
public class AllAuthorizationFilter : IDashboardAuthorizationFilter
{
    /// <summary>
    /// Метод авторизации.
    /// </summary>
    /// <param name="context">Контекст дашборда.</param>
    /// <returns><see langword="true"/> для всех случаев.</returns>
    public bool Authorize([NotNull] DashboardContext context) => true;
}