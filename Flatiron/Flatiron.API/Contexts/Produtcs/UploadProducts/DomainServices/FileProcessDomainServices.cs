namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public class FileProcessDomainServices(IFileProcessingQueue fileProcessingQueue,
                                       IFileReaderDomainServices fileReaderDomainServices,
                                       ICurrencyDomainServices CurrencyDomainServices,
                                       IProductRepository productRepository) : IFileProcessDomainServices
{
    public async Task ProcessFilesAsync(ILogServices logServices, CancellationToken cancellationToken)
    {
        var uploadedFiles = await fileProcessingQueue.DequeueAsync(cancellationToken);

        var processedProducts = new List<Product>();

        //reading files
        foreach (var uploadedFile in uploadedFiles)
        {
            var resultData = await fileReaderDomainServices.ReadProductFileAsync(uploadedFile);
            processedProducts.AddRange(resultData);
        }

        //get currencies from external services
        var newProducts = await CurrencyDomainServices.AddCurrenciesOnProductsAsync(processedProducts);

        if (newProducts.Any())
        {
            var CurrencyDate = newProducts.First().UploadedDate;
            var productCurrencies = newProducts.First().Currencies;

            //if has currencies on database, retrieve currencies and add on products
            //else insert the new currencies on database
            var existingCurrencies = await productRepository.GetCurrenciesByDateAsync(CurrencyDate);

            if (existingCurrencies.Any())
            {
                foreach (var newProduct in newProducts)
                {
                    newProduct.Currencies = [.. existingCurrencies];
                }
            }
            else
            {
                await productRepository.AddCurrenciesAsync(productCurrencies);
            }

            //saving product data and relationship with Currency

            await productRepository.AddProductsAsync(newProducts);

            logServices.WriteMessage("The file information was saved with success.");
        }
    }
}
