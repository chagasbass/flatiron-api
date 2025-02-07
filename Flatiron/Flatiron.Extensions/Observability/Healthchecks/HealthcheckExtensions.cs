namespace Flatiron.Extensions.Observability.Healthchecks;

public static class HealthcheckExtensions
{
    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
    {
        #region customs healthchecks 
        services.AddHealthChecks()
                .AddGCInfoCheck(HealthNames.MemoryHealthcheck, default, HealthNames.MemoryTags)
                .AddSelfCheck(HealthNames.SelfHealthcheck, default, HealthNames.SelfTags);

        #endregion

        return services;
    }

    public static IApplicationBuilder UseHealthChecksMiddleware(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHealthChecks(configuration);

        return app;
    }

    public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseHealthChecks("/healthz");
        app.UseHealthChecks("/healthz-json",
             new HealthCheckOptions()
             {
                 ResponseWriter = async (context, report) =>
                 {
                     string result = report.AddHealthStatusData(configuration);

                     context.Response.ContentType = MediaTypeNames.Application.Json;

                     await context.Response.WriteAsync(result);
                 }
             });

        return app;
    }
}
