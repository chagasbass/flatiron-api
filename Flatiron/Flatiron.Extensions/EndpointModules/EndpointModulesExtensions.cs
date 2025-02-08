using Asp.Versioning.Builder;
using Carter;
using Microsoft.AspNetCore.Routing;

namespace Flatiron.Extensions.EndpointModules;

public static class EndpointModulesExtensions
{
    public static IServiceCollection AddEndpointModuleExtensions(this IServiceCollection services)
    {
        services.AddCarter();

        return services;
    }

    public static WebApplication MapEndpointModules(this WebApplication app)
    {
        app.MapCarter();

        return app;
    }
}

public static class CarterVersionExtensions
{
    public static ApiVersionSet VersionEndpoints(IEndpointRouteBuilder app)
    {
        return app.NewApiVersionSet()
                   .HasApiVersion(new ApiVersion(1))
                   .ReportApiVersions()
                   .Build();
    }
}
