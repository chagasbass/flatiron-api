var builder = WebApplication.CreateBuilder(args);

Log.Logger = LogIntegrationsExtensions.ConfigureStructuralLogWithSerilog();
builder.Logging.AddSerilog(Log.Logger);

try
{
    var configuration = builder.Configuration;

    builder.Services.AddEndpointsApiExplorer()
                    .AddBaseConfigurationOptionsPattern(configuration)
                    .AddSwaggerDocumentation(configuration)
                    .AddLogServiceDependencies()
                    .AddNotificationControl()
                    .AddRequestResponseCompress()
                    .AddDependencyInjections()
                    .AddApiCustomResults()
                    .AddGlobalExceptionHandlerMiddleware()
                    .AddFilterToSystemLogs()
                    .AddMinimalApiVersionsing()
                    .AddAppHealthChecks();

    var app = builder.Build();

    #region Middleware Configurations

    app.UseResponseCompression()
       .UseExceptionHandler()
       .UseMiddleware<SerilogRequestLoggerMiddleware>()
       .UseSwagger()
       .UseSwaggerUI()
       .UseHttpsRedirection();

    app.UseHealthChecksMiddleware(configuration);

    #endregion

    app.AddWeatherV1Endpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host Ended.");
}
finally
{
    Log.CloseAndFlush();
}