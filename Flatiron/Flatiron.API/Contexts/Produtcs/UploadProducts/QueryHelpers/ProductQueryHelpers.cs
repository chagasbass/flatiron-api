using Dapper;
using Flatiron.API.Contexts.Produtcs.UploadProducts.Queries;
using System.Text;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.QueryHelpers;

public static class ProductQueryHelpers
{
    private static DynamicParameters CreateQueryFilters(ProductQuery productQuery)
    {
        var queryFilter = new DynamicParameters();

        if (!string.IsNullOrEmpty(productQuery.Name))
        {
            queryFilter.Add("Name", productQuery.Name, DbType.String);
        }

        if (productQuery.Expiration is not null)
        {
            queryFilter.Add("Expiration", productQuery.Expiration, DbType.DateTime);
        }

        if (productQuery.Price is not null)
        {
            queryFilter.Add("Price", productQuery.Price, DbType.Decimal);
        }

        return queryFilter;
    }

    private static void InsertQueryFilters(StringBuilder query, ProductQuery productQuery)
    {
        var hasName = false;
        var hasPrice = false;

        if (!string.IsNullOrEmpty(productQuery.Name))
        {
            query.Append(" WHERE Name = @Name");
            hasName = true;
        }

        if (productQuery.Price is not null && hasName)
        {
            query.Append(" AND Price = @price");
        }

        if (productQuery.Price is not null && !hasName)
        {
            query.Append(" WHERE Price = @price");
            hasPrice = true;
        }

        if (productQuery.Expiration is not null && hasName)
        {
            query.Append(" AND Expiration  = @Expiration");
        }

        if (productQuery.Expiration is not null && !hasName && !hasPrice)
        {
            query.Append(" WHERE Expiration  = @Expiration");
        }

        if (productQuery.Sorts.Any())
        {
            var orderByClause = string.Join(", ", productQuery.Sorts);

            query.AppendLine($"ORDER BY {orderByClause}");
        }
    }

    public static string AddProducts()
    {
        var query = new StringBuilder();
        query.AppendLine("INSERT INTO PRODUCTS (ID,NAME,PRICE,EXPIRATION) VALUES(@id,@name,@price,@expiration)");

        return query.ToString();
    }

    public static string GetProductsByFilters()
    {
        var query = new StringBuilder();

        query.AppendLine("SELECT NAME,PRICE,EXPIRATION, FROM PRODUCTS");

        return query.ToString();
    }
}
