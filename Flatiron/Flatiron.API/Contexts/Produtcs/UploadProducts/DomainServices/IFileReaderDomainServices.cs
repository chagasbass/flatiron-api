namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public interface IFileReaderDomainServices
{
    Task<IEnumerable<Product>> ReadProductFileAsync(UploadProductCommand command);
}
