using Flatiron.API.Contexts.Produtcs.UploadProducts.Entities;
using Flatiron.API.SharedFeatures.Models;
using Flatiron.Extensions.Shared.Configurations;
using Flurl.Http;
using Microsoft.Extensions.Options;

namespace Flatiron.API.SharedFeatures.Services;

public interface IConcurrencyExternalServices
{
    Task<IEnumerable<Product>> GetExchangeDataAsync(DateTime? expiration, List<Product> products);
}

public class ConcurrencyExternalServices(IOptions<BaseConfigurationOptions> options) : IConcurrencyExternalServices
{
    List<string> ConcurrencyKeys = new List<string> { "brl", "btc", "eur", "egp", "zeta" };

    public async Task<IEnumerable<Product>> GetExchangeDataAsync(DateTime? expiration, List<Product> products)
    {
        var concurrencyModelResult = await options.Value.ConcurrencyUrlService.GetJsonAsync<CurrencyResponse>();

        var concurrencyValues = concurrencyModelResult.Usd.Where(x => ConcurrencyKeys.Contains(x.Key))
            .Select(x => new Concurrency { Name = x.Key, Value = x.Value })
            .ToList();

        products.ForEach(product =>
        {
            product.AddConcurrencies(concurrencyValues);
        });

        return products;
    }
}

