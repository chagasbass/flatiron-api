namespace Flatiron.Extensions.Observability.Healthchecks;


public static class HealthCheckBuilderSelfExtensions
{
    public static IHealthChecksBuilder AddSelfCheck(this IHealthChecksBuilder builder, string name, HealthStatus? failureStatus = null, IEnumerable<string> tags = null)
    {
        builder.AddCheck<SelfHealthCheck>(name, failureStatus ?? HealthStatus.Degraded, tags);

        return builder;
    }
}
