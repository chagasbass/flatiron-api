using Flunt.Notifications;
using MediatR;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;

public class UploadProductCommand : Notifiable<Notification>, IRequest<ICommandResult>
{
    MemoryStream? FileStream { get; set; }
    public IFormFile? File { get; set; }

    public UploadProductCommand() { }

    public MemoryStream? GetStream() => FileStream;

    public async Task CreateFileStreamAsync(CancellationToken cancellationToken)
    {
        var memoryStream = new MemoryStream();
        await File.CopyToAsync(memoryStream, cancellationToken);

        memoryStream.Position = 0;

        FileStream = memoryStream;
    }

    public void Validate()
    {
        if (File == null || File.Length == 0)
        {
            AddNotification(new Notification("file-upload", "A file has required."));
        }

        var validFileExtensions = new[] { ".xls", ".xlsx", ".csv" };
        var fileExtension = Path.GetExtension(File.FileName).ToLowerInvariant();

        if (!validFileExtensions.Contains(fileExtension))
        {
            AddNotification(new Notification("file-upload", "Invalid File Extension.Only .xls , .xlsx or .csv are allowed."));
        }
    }
}
