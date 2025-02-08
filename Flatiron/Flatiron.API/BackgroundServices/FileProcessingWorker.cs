
using Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;
using Flatiron.Extensions.Shared.CustomsLogs.Services;

namespace Flatiron.API.BackgroundServices;

public class FileProcessingWorker(ILogServices logServices,
                                  IFileProcessDomainServices fileProcessDomainServices) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await fileProcessDomainServices.ProcessFilesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logServices.WriteMessage("An erro ocurried on file processing.");
                logServices.LogData.AddException(ex);
                logServices.WriteLogWhenRaiseExceptions();
            }
        }
    }
}
