namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Queries;

public class ProductQuery
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public DateTime? Expiration { get; set; }

    public List<string> Sorts { get; set; } = [];
    public ProductQuery() { }

}
