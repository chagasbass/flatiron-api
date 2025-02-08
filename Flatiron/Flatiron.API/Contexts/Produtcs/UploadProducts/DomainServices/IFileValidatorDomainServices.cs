using Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public interface IFileValidatorDomainServices
{
    void ValidateUploadFileAsync(UploadProductCommand uploadProductCommand);
}