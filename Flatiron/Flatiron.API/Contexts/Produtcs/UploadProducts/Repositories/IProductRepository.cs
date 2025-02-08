using Dapper;
using Flatiron.API.Contexts.Produtcs.UploadProducts.Entities;
using Flatiron.Extensions.Shared.Configurations;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Repositories;

public interface IProductRepository
{
    Task InitializeAsync();
    Task AddProductsAsync(IEnumerable<Product> products);
    Task<IEnumerable<Product>> GetProductsByFilterAsync();
}

public class ProductRepository(IOptions<BaseConfigurationOptions> options) : IProductRepository
{
    public async Task AddProductsAsync(IEnumerable<Product> products)
    {
        //using var connection = new SqliteConnection(options.Value.DataBaseConnectionString);
        //await connection.OpenAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByFilterAsync()
    {
        using var connection = new SqliteConnection(options.Value.DataBaseConnectionString);
        await connection.OpenAsync();

        return default;
    }

    public async Task InitializeAsync()
    {
        using var connection = new SqliteConnection(options.Value.DataBaseConnectionString);
        await connection.OpenAsync();

        var query = "CREATE TABLE IF NOT EXISTS PRODUCTS()";

        await connection.ExecuteAsync(query, commandType: CommandType.Text);
    }
}
