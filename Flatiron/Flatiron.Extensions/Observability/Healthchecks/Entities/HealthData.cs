namespace Flatiron.Extensions.Observability.Healthchecks.Entities;

public class HealthData
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }

    public HealthData() { }

}
