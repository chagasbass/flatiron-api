using Flatiron.API.Contexts.Produtcs.ListingProducts.Filters;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Repositories;

public interface IProductRepository
{
    Task AddProductsAsync(IEnumerable<Product> products);
    Task<IEnumerable<Product>> GetProductsByFilterAsync(ProductFilterQuery productFilterQuery);
    Task<IEnumerable<Currency>> GetCurrenciesByDateAsync(DateTime concurrencyDate);
    Task AddCurrenciesAsync(IEnumerable<Currency> currencies);
}