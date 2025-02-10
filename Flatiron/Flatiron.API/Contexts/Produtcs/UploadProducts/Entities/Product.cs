namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Entities;

public class Product : BaseEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public DateTime? Expiration { get; set; }
    public DateTime UploadedDate { get; set; }
    public List<Currency> Currencies { get; set; } = [];

    protected Product() { }

    public Product(string? name, decimal? price, DateTime? expiration)
    {
        Name = name;
        Price = price;
        Expiration = expiration.Value.Date;
        UploadedDate = DateTime.UtcNow;

        Validate();
    }

    public void AddCurrencies(List<Currency> currencies) => Currencies = currencies;

    public override void Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNullOrEmpty(Name, nameof(Name), "The name is required.")
            .IsGreaterThan(Price.Value, 0, "The price is required.")
            .IsNotNull(Expiration, nameof(Expiration), "The Expiration Date is required.")
            );
    }

    public static string? AddExpirationDate(string expirationDate)
    {
        if (!expirationDate.StartsWith("0"))
        {
            expirationDate = expirationDate.Insert(0, "0");
        }

        return expirationDate;
    }
}
