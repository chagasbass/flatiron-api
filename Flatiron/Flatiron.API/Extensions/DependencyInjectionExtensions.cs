using Flatiron.API.Contexts.Produtcs.UploadProducts.Behaviors;
using Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;
using Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;
using Flatiron.API.Contexts.Produtcs.UploadProducts.Repositories;
using Flatiron.API.SharedFeatures.Services;
using MediatR;
using System.Threading.Channels;

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

        services.AddScoped<IFileValidatorDomainServices, FileValidatorDomainServices>();
        services.AddTransient<IProductRepository, ProductRepository>();
        services.AddTransient<IFileProcessDomainServices, FileProcessDomainServices>();
        services.AddTransient<IFileReaderDomainServices, FileReaderDomainServices>();
        services.AddTransient<IConcurrencyDomainServices, ConcurrencyDomainServices>();
        services.AddTransient<IConcurrencyExternalServices, ConcurrencyExternalServices>();
        services.AddSingleton(Channel.CreateUnbounded<List<UploadProductCommand>>());
        services.AddSingleton<IFileProcessingQueue, FileProcessingQueue>();

        return services;
    }
}
