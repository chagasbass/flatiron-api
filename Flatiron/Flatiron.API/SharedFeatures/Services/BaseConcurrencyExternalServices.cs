namespace Flatiron.API.SharedFeatures.Services;

public abstract class BaseCurrencyExternalServices(IOptions<BaseConfigurationOptions> options)
{
    public List<string> CurrencyKeys = new List<string> { "brl", "btc", "eur", "egp", "zeta" };

    public string GenerateUrlService(DateTime uploadedDate)
    {
        return options.Value.CurrencyUrlService.Replace("{date}", uploadedDate.ToString("yyyy-MM-dd"));
    }
}

