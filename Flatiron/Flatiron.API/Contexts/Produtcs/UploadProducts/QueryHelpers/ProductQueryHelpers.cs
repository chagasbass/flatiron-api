using Flatiron.API.Contexts.Produtcs.ListingProducts.Filters;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.QueryHelpers;

public static class ProductQueryHelpers
{

    #region Filters

    private static void InsertQueryFilters(StringBuilder query, ProductFilterQuery productFilter)
    {
        var hasName = false;
        var hasPrice = false;

        if (!string.IsNullOrEmpty(productFilter.Name))
        {
            query.AppendLine(" WHERE p.Name LIKE @Name");
            hasName = true;
        }

        if (productFilter.Price is not null && hasName)
        {
            query.AppendLine(" AND p.Price <= @Price");
        }

        if (productFilter.Price is not null && !hasName)
        {
            query.AppendLine(" WHERE p.Price <= @Price");
            hasPrice = true;
        }

        if (productFilter.Expiration is not null && hasName)
        {
            query.AppendLine(" AND p.Expiration = @Expiration");
        }

        if (productFilter.Expiration is not null && !hasName && !hasPrice)
        {
            query.AppendLine(" WHERE p.Expiration  = @Expiration");
        }

        if (productFilter.Sorts.Any())
        {
            var orderByClause = string.Join(",", productFilter.Sorts.Select(sort => "p." + sort));

            query.AppendLine($"ORDER BY {orderByClause}");
        }
    }

    public static DynamicParameters CreateQueryFilters(ProductFilterQuery productFilter)
    {
        var queryFilter = new DynamicParameters();

        if (!string.IsNullOrEmpty(productFilter.Name))
        {
            var searchTerm = $"%{productFilter.Name}%";

            queryFilter.Add("Name", searchTerm, DbType.String);
        }

        if (productFilter.Expiration is not null)
        {
            queryFilter.Add("Expiration", productFilter.Expiration, DbType.DateTime);
        }

        if (productFilter.Price is not null)
        {
            queryFilter.Add("Price", productFilter.Price, DbType.Decimal);
        }

        return queryFilter;
    }

    public static object PrepareCurrencyData(IEnumerable<Currency> currencies)
    {
        var datas = currencies.Select(currency => new
        {
            id = currency.CurrencyId,
            name = currency.CurrencyName,
            value = currency.CurrencyValue,
            currencyDate = currency.CurrencyDate,
        }).ToList();

        return datas;
    }

    public static object PrepareProductData(IEnumerable<Product> products)
    {
        var datas = products.Select(product => new
        {
            id = product.Id,
            name = product.Name,
            price = product.Price,
            expiration = product.Expiration,
            uploadedDate = product.UploadedDate,
        }).ToList();

        return datas;
    }

    public static object PrepareProductCurrencyData(IEnumerable<Product> products)
    {
        var productsCurrencies = products

       .SelectMany(product => product.Currencies.Select(currency => new
       {
           productId = product.Id,
           currencyId = currency.CurrencyId
       }))
       .Cast<object>()
       .ToList();

        return productsCurrencies;
    }

    #endregion

    #region Queries
    public static string AddProducts()
    {
        var query = new StringBuilder();
        query.AppendLine("INSERT INTO Products (Id,Name,Price,Expiration,UploadedDate) VALUES(@id,@name,@price,@expiration,@uploadedDate);");

        return query.ToString();
    }

    public static string GetCurrencies()
    {
        var query = new StringBuilder();
        query.AppendLine("SELECT CurrencyId,CurrencyName,CurrencyValue,CurrencyDate FROM Currencies WHERE CurrencyDate = @date");

        return query.ToString();
    }

    public static string AddProductCurrencies()
    {
        var query = new StringBuilder();
        query.AppendLine("INSERT INTO ProductsCurrencies (ProductId,CurrencyId) VALUES(@productId,@currencyId);");

        return query.ToString();
    }

    public static string AddCurrencies()
    {
        var query = new StringBuilder();
        query.AppendLine("INSERT INTO Currencies (CurrencyId,CurrencyName,CurrencyValue,CurrencyDate) VALUES(@id,@name,@value,@currencyDate);");
        return query.ToString();
    }

    public static string GetProductsByFilters(ProductFilterQuery productFilter)
    {
        var query = new StringBuilder();

        query.AppendLine(" SELECT p.Id, p.Name, p.Price, p.Expiration, p.UploadedDate,c.CurrencyId, c.CurrencyName, c.CurrencyValue");
        query.AppendLine(" FROM Products p LEFT JOIN ProductsCurrencies pc ON p.Id = pc.ProductId ");
        query.AppendLine(" LEFT JOIN Currencies c ON pc.CurrencyId = c.CurrencyId");

        InsertQueryFilters(query, productFilter);

        return query.ToString();
    }

    #endregion


}
