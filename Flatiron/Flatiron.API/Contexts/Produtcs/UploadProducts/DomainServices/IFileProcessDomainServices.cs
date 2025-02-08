using Flatiron.API.Contexts.Produtcs.UploadProducts.Entities;
using Flatiron.API.Contexts.Produtcs.UploadProducts.Repositories;
using Flatiron.API.SharedFeatures.Services;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public interface IFileProcessDomainServices
{
    Task ProcessFilesAsync(CancellationToken cancellationToken);
}

public class FileProcessDomainServices(IFileProcessingQueue fileProcessingQueue,
                                       IFileReaderDomainServices fileReaderDomainServices,
                                       IConcurrencyDomainServices concurrencyDomainServices,
                                       IProductRepository productRepository) : IFileProcessDomainServices
{
    public async Task ProcessFilesAsync(CancellationToken cancellationToken)
    {
        var uploadedFiles = await fileProcessingQueue.DequeueAsync(cancellationToken);

        var groupedProducts = new List<IGrouping<DateTime?, Product>>();

        //read files
        foreach (var uploadedFile in uploadedFiles)
        {
            var resultData = await fileReaderDomainServices.ReadProductFileAsync(uploadedFile);
            groupedProducts.AddRange(resultData);
        }

        //get concurrencies
        //var newProducts = await concurrencyDomainServices.AddConcurrenciesOnProductsAsync(groupedProducts);

        //save data
        // await productRepository.AddProductsAsync(newProducts);
    }
}
