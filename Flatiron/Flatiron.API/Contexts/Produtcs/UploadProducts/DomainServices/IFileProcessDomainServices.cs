namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public interface IFileProcessDomainServices
{
    Task ProcessFilesAsync(ILogServices logServices, CancellationToken cancellationToken);
}
