namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public class CurrencyDomainServices(ILogServices logServices,
                                       ICurrencyExternalServices CurrencyExternalServices) : ICurrencyDomainServices
{
    public async Task<IEnumerable<Product>> AddCurrenciesOnProductsAsync(IEnumerable<Product> products)
    {
        var newProducts = new List<Product>();

        try
        {
            var productsWithCurrency = await CurrencyExternalServices.GetExchangeDataAsync(products.ToList());
            return productsWithCurrency;
        }
        catch (Exception ex)
        {
            logServices.WriteMessage("An error ocurried on getting currencies for products. The file will not be processed.");
            logServices.LogData.AddException(ex);
            logServices.WriteLogWhenRaiseExceptions();

            return Enumerable.Empty<Product>();
        }
    }
}
