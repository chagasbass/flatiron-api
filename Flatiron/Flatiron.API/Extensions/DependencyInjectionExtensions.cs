namespace Flatiron.API.Extensions;
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<Program>();

            config.AddBehavior<IPipelineBehavior<UploadProductCommand, ICommandResult>,
          UploadProductFileBehaviors<UploadProductCommand, ICommandResult>>();
        });

        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IFileProcessDomainServices, FileProcessDomainServices>();
        services.AddTransient<IFileReaderDomainServices, FileReaderDomainServices>();
        services.AddTransient<ICurrencyDomainServices, CurrencyDomainServices>();
        services.AddTransient<ICurrencyExternalServices, CurrencyExternalServices>();
        services.AddSingleton(Channel.CreateUnbounded<List<UploadProductCommand>>());
        services.AddSingleton<IFileProcessingQueue, FileProcessingQueue>();
        services.AddSingleton<ICacheService<IEnumerable<Currency>>, MemoryCacheService<IEnumerable<Currency>>>();

        return services;
    }
}
