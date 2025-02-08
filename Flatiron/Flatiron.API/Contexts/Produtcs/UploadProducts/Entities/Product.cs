using Flunt.Notifications;
using Flunt.Validations;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Entities;

public class Product : BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public DateTime? Expiration { get; set; }
    public List<Concurrency> Concurrencies { get; set; } = [];

    public Product(string? name, decimal? price, DateTime? expiration)
    {
        Name = name;
        Price = price;
        Expiration = expiration;

        Validate();
    }

    public void AddConcurrencies(List<Concurrency> concurrencies) => Concurrencies = concurrencies;

    public override void Validate()
    {
        decimal numericalPrice = 0;

        //if (string.IsNullOrEmpty(price))
        //{
        //    var newPrice = price.Replace("$", "");
        //    var hasPrice = Decimal.TryParse(newPrice, out numericalPrice);

        //    if (hasPrice)
        //    {
        //        Price = numericalPrice;
        //    }
        //    else
        //    {
        //        AddNotification(new Notification(nameof(Price), "The price is invalid"));
        //    }
        //}
        //else
        //{
        //    AddNotification(new Notification(nameof(Price), "The price is invalid"));
        //}

        AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNullOrEmpty(Name, nameof(Name), "The name is required.")
            .IsGreaterThan(Price.Value, 0, "The price is required.")
            .IsNotNull(Expiration, nameof(Expiration), "The Expiration Date is required.")
            );
    }
}

public class Concurrency
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal? Value { get; set; }

    public Concurrency()
    {
        Id = Guid.NewGuid();
    }
}
