namespace Flatiron.API.SharedFeatures.Services;

public interface IFileProcessingQueue
{
    ValueTask QueueFilesAsync(List<UploadProductCommand> commands, CancellationToken cancellationToken);
    ValueTask<List<UploadProductCommand>> DequeueAsync(CancellationToken cancellationToken);
}

