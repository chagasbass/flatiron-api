namespace Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;

public class FileReaderDomainServices(ILogServices logServices, ICurrencyExternalServices CurrencyExternalServices) : IFileReaderDomainServices
{
    public async Task<IEnumerable<Product>> ReadProductFileAsync(UploadProductCommand command)
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        var products = new List<Product>();

        var fileExtension = Path.GetExtension(command.File.FileName).ToLower();

        int fileErros = 0;

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

                string? expirationDate = Product.AddExpirationDate(columns[2].ToString());

                var product = new Product(
                    columns[0]?.Trim(),
                    decimal.TryParse(columns[1]?.Replace("$", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out var price) ? price : 0,
                    DateTime.TryParseExact(expirationDate, "MM/dd/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var expiration) ? expiration : DateTime.MinValue
                );

                if (!product.IsValid)
                {
                    fileErros++;
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

            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];

                string? expirationDate = Product.AddExpirationDate(row[2]?.ToString());

                var product = new Product(
                    row[0]?.ToString(),
                    decimal.TryParse(row[1]?.ToString().Replace("$", ""), out var price) ? price : 0,
                    DateTime.TryParseExact(expirationDate, "MM/dd/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var expiration) ? expiration : DateTime.MinValue);

                if (!product.IsValid)
                {
                    fileErros++;
                }

                products.Add(product);

                fileLine++;
            }
        }
        #endregion

        logServices.WriteMessage($"We have encountered {fileErros} lines with errors on file {command.File.Name}.This products will not be processeds.");

        return products;
    }
}
