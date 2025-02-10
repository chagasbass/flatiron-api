namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public interface ICurrencyDomainServices
{
    Task<IEnumerable<Product>> AddCurrenciesOnProductsAsync(IEnumerable<Product> products);
}
