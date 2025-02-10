namespace Flatiron.API.SharedFeatures.Services;

public interface ICurrencyExternalServices
{
    Task<IEnumerable<Product>> GetExchangeDataAsync(List<Product> products);
}

