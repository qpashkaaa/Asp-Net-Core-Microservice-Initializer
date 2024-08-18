using AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services;

[AutoRegisterService(ServiceLifetime.Transient)]
public class RandomService
{
    private readonly IRandomGuidService _randomGuidService;
    private readonly IRandomNumberService _randomNumberService;
    private readonly IRandomWordService _randomWordService;

    public RandomService(
        IRandomGuidService randomGuidService,
        IRandomNumberService randomNumberService,
        IRandomWordService randomWordService)
    {
        _randomGuidService = randomGuidService;
        _randomNumberService = randomNumberService;
        _randomWordService = randomWordService;
    }

    public Dictionary<string, object> GetRandomValues()
    {
        return new Dictionary<string, object>
        {
            { "Guid", _randomGuidService.GetRandomGuid() },
            { "Number", _randomNumberService.GetRandomNumber() },
            { "Word", _randomWordService.GetRandomWord() }
        };
    }
}
