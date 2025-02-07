namespace Flatiron.Extensions.Options;

public static class OptionsExtensions
{
    public static IServiceCollection AddBaseConfigurationOptionsPattern(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<BaseConfigurationOptions>(configuration.GetSection(BaseConfigurationOptions.BaseConfig));
        services.Configure<ProblemDetailConfigurationOptions>(configuration.GetSection(ProblemDetailConfigurationOptions.BaseConfig));
        services.Configure<ResilienceConfigurationOptions>(configuration.GetSection(ResilienceConfigurationOptions.ResilienciaConfig));

        return services;
    }
}
