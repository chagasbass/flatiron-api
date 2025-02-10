namespace Flatiron.Extensions.Shared.Configurations;

public class BaseConfigurationOptions
{
    public const string BaseConfig = "BaseConfiguration";
    public string? ApplicationName { get; set; }
    public string? Description { get; set; }
    public string? Developer { get; set; }
    public string? DataBaseConnectionString { get; set; }
    public string? CurrencyUrlService { get; set; }

    public BaseConfigurationOptions() { }

}
