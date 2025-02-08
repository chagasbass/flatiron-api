using Flatiron.API.BackgroundServices;
using Flatiron.Extensions.EndpointModules;

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
                    .AddEndpointModuleExtensions()
                    .AddMinimalApiVersionsing()
                    .AddAppHealthChecks();

    builder.Services.AddHostedService<FileProcessingWorker>();

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
    app.MapEndpointModules();
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