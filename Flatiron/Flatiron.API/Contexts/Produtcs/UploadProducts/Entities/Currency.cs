namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Entities;

public class Currency
{
    public string CurrencyId { get; set; }
    public DateTime CurrencyDate { get; set; }
    public string? CurrencyName { get; set; }
    public decimal? CurrencyValue { get; set; }

    public Currency()
    {
        CurrencyId = Guid.NewGuid().ToString();
    }

}
