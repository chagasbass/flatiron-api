namespace Flatiron.API.Contexts.Produtcs.ListingProducts.Filters;

public class ProductFilterQuery : IRequest<ICommandResult>
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public DateTime? Expiration { get; set; }

    public string[]? Sorts { get; set; } = [];

    List<string> _sorts = ["Name", "Price", "Expiration"];

    public ProductFilterQuery() { }

    public Notification Validate()
    {
        if (Sorts.Any())
        {
            var invalidSorts = Sorts
                    .Where(v => !_sorts.Contains(v, StringComparer.OrdinalIgnoreCase))
                    .ToList();

            if (invalidSorts.Any())
            {
                return new Notification("sort-property", "There is an invalid sort on filter.");
            }
        }

        var validSorts = Sorts
                    .Where(v => _sorts.Contains(v, StringComparer.OrdinalIgnoreCase))
                    .Select(v => _sorts.First(valid => valid.Equals(v, StringComparison.OrdinalIgnoreCase)));

        Sorts = [];
        Sorts = validSorts.ToArray();

        return default;
    }
}
