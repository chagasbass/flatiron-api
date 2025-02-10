namespace Flatiron.API.Contexts.Produtcs.ListingProducts.Queries;

public class ProductQuery
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public DateTime? Expiration { get; set; }
    public DateTime UploadedDate { get; set; }
    public List<CurrencyQuery> Currencies { get; set; } = [];

    public ProductQuery() { }

    public static implicit operator ProductQuery(Product product)
    {
        var currencies = new List<CurrencyQuery>();

        product.Currencies.ForEach(currency =>
        {
            CurrencyQuery currencyQuery = currency;
            currencies.Add(currencyQuery);
        });

        return new ProductQuery
        {
            Name = product.Name,
            Price = product.Price,
            Expiration = product.Expiration,
            UploadedDate = product.UploadedDate,
            Currencies = currencies
        };
    }

}

public class CurrencyQuery
{
    public string? Name { get; set; }
    public decimal? Value { get; set; }

    public CurrencyQuery() { }

    public static implicit operator CurrencyQuery(Currency currency)
    {
        return new CurrencyQuery
        {
            Name = currency.CurrencyName,
            Value = currency.CurrencyValue
        };
    }
}

