using Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public class FileValidatorDomainServices(INotificationServices notificationServices) : IFileValidatorDomainServices
{
    public void ValidateUploadFileAsync(UploadProductCommand uploadProductCommand)
    {
        uploadProductCommand.Validate();

        if (!uploadProductCommand.IsValid)
        {
            notificationServices.AddNotifications(uploadProductCommand.Notifications, StatusCodeOperation.BusinessError);
        }
    }
}
