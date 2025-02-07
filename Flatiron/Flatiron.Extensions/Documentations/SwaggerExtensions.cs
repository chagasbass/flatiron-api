namespace Flatiron.Extensions.Documentations;

public static class SwaggerExtensions
{
    const string standardMessage = "Not informed";

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationName = configuration["BaseConfiguration:ApplicationName"];
        var applicationDescription = configuration["BaseConfiguration:Description"];
        var developerName = configuration["BaseConfiguration:Developer"];

        if (string.IsNullOrEmpty(applicationName))
            applicationName = standardMessage;

        if (string.IsNullOrEmpty(developerName))
            developerName = standardMessage;

        var info = new OpenApiInfo
        {
            Title = applicationName,
            Description = $"{applicationDescription} Developed by {developerName}"
        };

        services.AddSwaggerGen(delegate (SwaggerGenOptions c)
        {
            #region Resolver conflitos de rotas
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            #endregion

            c.SwaggerDoc("v1", info);

            c.EnableAnnotations();
        });

        return services;
    }
}