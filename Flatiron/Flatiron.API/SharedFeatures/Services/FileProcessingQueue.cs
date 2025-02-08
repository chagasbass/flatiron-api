using Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;
using System.Threading.Channels;

namespace Flatiron.API.SharedFeatures.Services;

public class FileProcessingQueue(Channel<List<UploadProductCommand>> queue) : IFileProcessingQueue
{
    public async ValueTask<List<UploadProductCommand>> DequeueAsync(CancellationToken cancellationToken)
    {
        var data = await queue.Reader.ReadAsync(cancellationToken);

        return data;
    }

    public async ValueTask QueueFilesAsync(List<UploadProductCommand> commands, CancellationToken cancellationToken)
    {
        await queue.Writer.WriteAsync(commands, cancellationToken);
    }
}

