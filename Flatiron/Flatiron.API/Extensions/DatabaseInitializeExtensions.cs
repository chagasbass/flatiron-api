namespace Flatiron.API.Extensions;

public static class DatabaseInitializeExtensions
{
    static string CreateTables()
    {
        var query = new StringBuilder();
        query.AppendLine(" PRAGMA foreign_keys = ON;");
        query.AppendLine(" CREATE TABLE IF NOT EXISTS Products (Id TEXT  PRIMARY KEY, Name TEXT,Price REAL,Expiration DATE,UploadedDate DATETIME);");
        query.AppendLine(" CREATE TABLE IF NOT EXISTS Currencies (CurrencyId TEXT PRIMARY KEY,CurrencyName TEXT,CurrencyValue REAL,CurrencyDate DATE);");
        query.AppendLine(" CREATE TABLE IF NOT EXISTS ProductsCurrencies (ProductId TEXT NOT NULL,CurrencyId TEXT NOT NULL, PRIMARY KEY (ProductId, CurrencyId),");
        query.AppendLine(" FOREIGN KEY (ProductId) REFERENCES Products(Id) ON DELETE CASCADE,");
        query.AppendLine(" FOREIGN KEY (CurrencyId) REFERENCES Currencies(CurrencyId) ON DELETE CASCADE);");

        return query.ToString();
    }

    public static void AddDatabaseInitialer(IConfiguration configuration)
    {
        var connectionString = configuration["BaseConfiguration:DataBaseConnectionString"];

        Batteries_V2.Init();

        var query = CreateTables();

        using var connection = new SqliteConnection(connectionString);
        connection.Open();
        connection.Execute(query);
    }
}
