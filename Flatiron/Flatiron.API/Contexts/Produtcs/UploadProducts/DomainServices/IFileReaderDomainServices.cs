using ExcelDataReader;
using Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;
using Flatiron.API.Contexts.Produtcs.UploadProducts.Entities;
using Flatiron.API.SharedFeatures.Services;
using Flatiron.Extensions.Shared.CustomsLogs.Services;
using System.Globalization;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public interface IFileReaderDomainServices
{
    Task<List<IGrouping<DateTime?, Product>>> ReadProductFileAsync(UploadProductCommand command);
}

public class FileReaderDomainServices(ILogServices logServices, IConcurrencyExternalServices concurrencyExternalServices) : IFileReaderDomainServices
{
    /// <summary>
    /// TODO: Refactor ( Se sobrar tempo usar um strategy para cada fileextension)
    /// Criar um Enum para as extensões
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public async Task<List<IGrouping<DateTime?, Product>>> ReadProductFileAsync(UploadProductCommand command)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var products = new List<Product>();

        var fileExtension = Path.GetExtension(command.File.FileName).ToLower();

        #region Reading csv File

        if (fileExtension == ".csv")
        {
            using var readerCsv = new StreamReader(command.GetStream());

            string line;
            int fileLineCsv = 2;

            await readerCsv.ReadLineAsync();

            while ((line = await readerCsv.ReadLineAsync()) != null)
            {
                var columns = line.Split(';');

                var product = new Product(
                    columns[0]?.Trim(),
                    decimal.TryParse(columns[1]?.Replace("$", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var price) ? price : 0,
                    DateTime.TryParse(columns[2], out var expiration) ? expiration : DateTime.MinValue
                );

                if (!product.IsValid)
                {
                    logServices.WriteMessage($"An error occurred on file {command.File.FileName} on line {fileLineCsv}. The line will be ignored.");
                }
                else
                {
                    products.Add(product);
                }

                fileLineCsv++;
            }
        }
        #endregion

        #region Reading xls or xlsx files
        else if (fileExtension == ".xls" || fileExtension == ".xlsx")
        {

            using var reader = ExcelReaderFactory.CreateReader(command.GetStream());

            var result = reader.AsDataSet();
            var table = result.Tables[0];
            var fileLine = 2;

            // Começa na linha 1 para pular o cabeçalho
            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];

                var product = new Product(
                    row[0]?.ToString(),
                    decimal.TryParse(row[1]?.ToString().Replace("$", ""), out var price) ? price : 0,
                    DateTime.TryParse(row[2]?.ToString(), out var expiration) ? expiration : DateTime.MinValue);

                if (!product.IsValid)
                {
                    logServices.WriteMessage($"An erro has encontered on file {command.File.FileName} on line {fileLine}.The file will not be processed.");
                    return new();
                }

                products.Add(product);

                fileLine++;
            }
        }
        #endregion

        var groupProducts = products.GroupBy(x => x.Expiration).ToList();

        return groupProducts;
    }
}

public interface IConcurrencyDomainServices
{
    Task<IEnumerable<Product>> AddConcurrenciesOnProductsAsync(List<IGrouping<DateTime?, Product>> groupedProducts);
}

public class ConcurrencyDomainServices(ILogServices logServices,
                                       IConcurrencyExternalServices concurrencyExternalServices) : IConcurrencyDomainServices
{
    public async Task<IEnumerable<Product>> AddConcurrenciesOnProductsAsync(List<IGrouping<DateTime?, Product>> groupedProducts)
    {
        var newProducts = new List<Product>();

        var semaphore = new SemaphoreSlim(50);

        var concurreciesTasks = groupedProducts.Select(async group =>
        {
            await semaphore.WaitAsync();

            try
            {
                var products = await concurrencyExternalServices.GetExchangeDataAsync(group.Key.Value, group.ToList());
                return products;
            }
            catch (Exception ex)
            {
                logServices.WriteMessage("An error ocurried on getting concurrencies for products. The file will not be processed.");
                logServices.LogData.AddException(ex);
                logServices.WriteLogWhenRaiseExceptions();

                return Enumerable.Empty<Product>();
            }
            finally
            {
                semaphore.Release();
            }
        });

        var results = await Task.WhenAll(concurreciesTasks);

        foreach (var resultItem in results)
        {
            newProducts.AddRange(resultItem);
        }

        return newProducts;
    }
}
