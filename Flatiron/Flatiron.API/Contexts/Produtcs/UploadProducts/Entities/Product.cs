using Flunt.Notifications;
using Flunt.Validations;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Entities;

public class Product(string? name, decimal? price, DateTime? expiration) : BaseEntity
{
    public string? Name { get; set; } = name;
    public decimal? Price { get; set; } = price;
    public DateTime? Expiration { get; set; } = expiration;

    public override void Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNullOrEmpty(Name, nameof(Name), "The name is required.")
            .IsGreaterThan(Price.Value, 0, "The price is required.")
            .IsNotNull(Expiration, nameof(Expiration), "The Expiration Date is required.")
            );
    }
}

public static class ProductFactory
{
    public static Product CreateNewProduct(string? name, string? price, DateTime? expiration)
    {
        decimal newPrice = 0;

        if (string.IsNullOrEmpty(price))
        {
            newPrice = decimal.Parse(price.Replace("$", ""));
        }

        return new Product(name, newPrice, expiration);

    }
}
