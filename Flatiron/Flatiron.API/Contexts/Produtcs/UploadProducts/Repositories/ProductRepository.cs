using Flatiron.API.Contexts.Produtcs.ListingProducts.Filters;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Repositories;

public class ProductRepository(ILogServices logServices,
                               IOptions<BaseConfigurationOptions> options) : IProductRepository
{
    public async Task AddCurrenciesAsync(IEnumerable<Currency> currencies)
    {
        var newCurrencies = ProductQueryHelpers.PrepareCurrencyData(currencies);

        using var connection = new SqliteConnection(options.Value.DataBaseConnectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var retrievedCurrencies = await connection.ExecuteAsync(ProductQueryHelpers.AddCurrencies(),
                                                                    newCurrencies, transaction,
                                                                    commandType: CommandType.Text);
            await transaction.CommitAsync();

        }
        catch (Exception ex)
        {
            logServices.WriteMessage("The application have encounter an error on saving Currencies file data.The file will not be processed, Please make the upload again.");
            logServices.LogData.AddException(ex);
            logServices.WriteLogWhenRaiseExceptions();
            await transaction.RollbackAsync();
        }
    }

    public async Task AddProductsAsync(IEnumerable<Product> products)
    {
        var productData = ProductQueryHelpers.PrepareProductData(products);
        var productCurrencies = ProductQueryHelpers.PrepareProductCurrencyData(products);

        using var connection = new SqliteConnection(options.Value.DataBaseConnectionString);
        await connection.OpenAsync();

        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            await connection.ExecuteAsync(ProductQueryHelpers.AddProducts(), productData, transaction, commandType: CommandType.Text);
            await connection.ExecuteAsync(ProductQueryHelpers.AddProductCurrencies(), productCurrencies, transaction, commandType: CommandType.Text);

            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            logServices.WriteMessage("The application have encounter an error on saving  products file data.The file will not be processed, Please make the upload again.");
            logServices.LogData.AddException(ex);
            logServices.WriteLogWhenRaiseExceptions();
            await transaction.RollbackAsync();
        }
    }

    public async Task<IEnumerable<Currency>> GetCurrenciesByDateAsync(DateTime concurrencyDate)
    {
        var date = new
        {
            date = concurrencyDate
        };

        using var connection = new SqliteConnection(options.Value.DataBaseConnectionString);
        await connection.OpenAsync();

        var currencies = await connection.QueryAsync<Currency>(ProductQueryHelpers.GetCurrencies(),
                                                                     date, commandType: CommandType.Text);

        return currencies;
    }

    public async Task<IEnumerable<Product>> GetProductsByFilterAsync(ProductFilterQuery productFilterQuery)
    {
        var queryFilters = ProductQueryHelpers.CreateQueryFilters(productFilterQuery);
        var query = ProductQueryHelpers.GetProductsByFilters(productFilterQuery);

        var productDictionary = new Dictionary<string, Product>();

        using var connection = new SqliteConnection(options.Value.DataBaseConnectionString);
        await connection.OpenAsync();

        var products = await connection.QueryAsync<Product, Currency, Product>(
        query,
        (product, currency) =>
        {

            if (!productDictionary.TryGetValue(product.Id, out var currentProduct))
            {
                currentProduct = product;
                currentProduct.Currencies = new List<Currency>();
                productDictionary.Add(product.Id, currentProduct);
            }

            if (currency != null && currency.CurrencyId != Guid.Empty.ToString())
            {
                currentProduct.Currencies.Add(currency);
            }

            return currentProduct;
        },
        param: queryFilters,
        splitOn: "CurrencyId"
    );

        return productDictionary.Values.ToList();
    }
}
