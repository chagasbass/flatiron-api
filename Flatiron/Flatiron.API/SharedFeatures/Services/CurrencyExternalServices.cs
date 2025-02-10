namespace Flatiron.API.SharedFeatures.Services;

public class CurrencyExternalServices(IOptions<BaseConfigurationOptions> options,
                                        ICacheService<IEnumerable<Currency>> cacheService)
                                        : BaseCurrencyExternalServices(options),
                                        ICurrencyExternalServices
{
    public async Task<IEnumerable<Product>> GetExchangeDataAsync(List<Product> products)
    {
        var cachedCurrencies = cacheService.Get();

        if (cachedCurrencies is not null)
        {
            products.ForEach(product =>
            {
                product.AddCurrencies(cachedCurrencies.ToList());
            });
        }
        else
        {
            var uploadedDate = products.First().UploadedDate;

            var urlService = GenerateUrlService(uploadedDate);

            var CurrencyModelResult = await urlService.GetJsonAsync<CurrencyResponse>();

            var CurrencyDate = DateTime.Parse(CurrencyModelResult.Date);

            var CurrencyValues = CurrencyModelResult.Usd.Where(x => CurrencyKeys.Contains(x.Key))
                .Select(x => new Currency { CurrencyName = x.Key, CurrencyValue = x.Value, CurrencyDate = CurrencyDate })
                .ToList();

            cacheService.Set(CurrencyValues);

            products.ForEach(product =>
            {
                product.AddCurrencies(CurrencyValues);
            });
        }

        return products;
    }
}

