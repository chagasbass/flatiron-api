namespace Flatiron.Extensions.Observability.Healthchecks;

public static class HealthReportExtensions
{
    public static string AddHealthStatusData(this HealthReport report, IConfiguration configuration)
    {
        var applicationName = configuration["BaseConfiguration:ApplicationName"];

        var healthInformation = new HealthInformation
        {
            Name = applicationName,
            Data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
        };

        var entries = report.Entries.ToList();

        foreach (var entrie in entries)
        {
            if (entrie.Key.Equals(HealthNames.MemoryHealthcheck))
            {
                healthInformation.MemoryInformation = new MemoryInformation
                {
                    Name = entrie.Key,
                    Description = entrie.Value.Description,
                    Status = entrie.Value.Status.ToString(),
                    AllocatedMemory = GCInfoOptions.AllocatedMemory,
                    TotalAvailableMemory = GCInfoOptions.TotalAvailableMemory,
                    MaxMemory = GCInfoOptions.MaxMemory,
                    OperationalSystem = GCInfoOptions.OperationalSystem,
                    OperationalSystemArchitecture = GCInfoOptions.OperationalSystemArchitecture,
                    ApplicationFramework = GCInfoOptions.ApplicationFramework
                };
            }
            else
            {
                healthInformation.HealthDatas.Add(new HealthData
                {
                    Name = entrie.Key,
                    Description = entrie.Value.Description,
                    Status = entrie.Value.Status.ToString()
                });
            }
        }

        var serializeOptions = JsonOptionsFactory.GetSerializerOptions();

        var healthResult = JsonSerializer.Serialize(healthInformation, serializeOptions);

        return healthResult;
    }
}
