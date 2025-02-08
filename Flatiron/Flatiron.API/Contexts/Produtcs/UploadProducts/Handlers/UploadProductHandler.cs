using Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;
using Flatiron.API.SharedFeatures.Services;
using MediatR;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Handlers;

public class UploadProductHandler(INotificationServices notificationServices,
                                  IFileProcessingQueue fileProcessingQueue) : IRequestHandler<UploadProductCommand, ICommandResult>
{
    public async Task<ICommandResult> Handle(UploadProductCommand request, CancellationToken cancellationToken)
    {
        await request.CreateFileStreamAsync(cancellationToken);

        var uploadedFiles = new List<UploadProductCommand>() { request };

        await fileProcessingQueue.QueueFilesAsync(uploadedFiles, cancellationToken);

        notificationServices.AddStatusCode(StatusCodeOperation.Created);

        return new CommandResult(true, "Success on file upload.");
    }
}
