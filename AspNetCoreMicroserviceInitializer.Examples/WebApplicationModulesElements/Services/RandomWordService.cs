using System.Text;
using AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services.Interfaces;
using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Interfaces;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services;

[AutoRegisterService(ServiceLifetime.Singleton, typeof(IRandomWordService))]
public class RandomWordService : IRandomWordService, IServiceImplementationFactory<RandomWordService>
{
    private readonly char[] _characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();

    protected virtual int MaxWordLength { get; } = 125;

    /// <summary>
    /// A factory function,which is added using <see cref="IServiceImplementationFactory{TService}"/> that will be used when registering the service and return <see cref="RandowShortWordService"/>.
    /// </summary>
    /// <remarks>
    /// For a test, you can comment out this line and remove the interface from the class. And you will be able to observe the generation of random words of more than 5 characters.
    /// </remarks>
    public Func<IServiceProvider, RandomWordService> ImplementationFactory => (sp) => new RandowShortWordService();

    public virtual string GetRandomWord()
    {
        var wordLength = new Random().Next(1, MaxWordLength);

        return GenerateRandowWord(wordLength);
    }

    protected string GenerateRandowWord(int wordLength)
    {
        StringBuilder wordBuilder = new StringBuilder();

        for (int i = 0; i < wordLength; i++)
        {
            int randomIndex = new Random().Next(_characters.Length);
            wordBuilder.Append(_characters[randomIndex]);
        }

        return wordBuilder.ToString();
    }
}
