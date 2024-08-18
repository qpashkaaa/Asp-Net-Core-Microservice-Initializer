namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Services;

public class RandowShortWordService : RandomWordService
{
    protected override int MaxWordLength => 5;

    public override string GetRandomWord()
    {
        return base.GetRandomWord();
    }
}
