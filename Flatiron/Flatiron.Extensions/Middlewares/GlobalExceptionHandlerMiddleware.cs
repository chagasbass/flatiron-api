namespace Flatiron.Extensions.Middlewares;

public sealed class GlobalExceptionHandlerMiddleware(IOptions<ProblemDetailConfigurationOptions> problemOptions,
                                                     ILogServices logServices) : IExceptionHandler
{
    /// <summary>
    /// Responsavel por tratar as exceções geradas na aplicação
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    public async Task HandleExceptionAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is null)
            return;

        const string dataType = @"application/problem+json";
        const int statusCode = StatusCodes.Status500InternalServerError;

        logServices.LogData.AddException(exception)
                           .AddResponseStatusCode(statusCode);

        logServices.WriteLog();
        logServices.WriteLogWhenRaiseExceptions();

        var problemDetails = ConfigureProblemDetails(statusCode, context);

        var commandResult = new CommandResult(problemDetails);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = dataType;

        var jsonResponse = JsonSerializer.Serialize(commandResult, JsonOptionsFactory.GetSerializerOptions());

        await context.Response.WriteAsync(jsonResponse, cancellationToken);
    }

    private ProblemDetails ConfigureProblemDetails(int statusCode, HttpContext context)
    {
        var defaultTitle = $"An error ocurried when request was processed at {DateTimeExtensions.GetGmtDateTime(DateTime.UtcNow)}";
        var defaultDetail = $"Fatal Error on aplication contact a Developer";

        var title = problemOptions.Value.Title;
        var detail = problemOptions.Value.Detail;
        var instance = context.Request.HttpContext.Request.Path.ToString();

        var type = StatusCodeOperation.RetrieveStatusCode(statusCode);

        if (string.IsNullOrEmpty(title))
            title = defaultTitle;

        if (string.IsNullOrEmpty(detail))
            detail = defaultDetail;

        return new ProblemDetails()
        {
            Detail = detail,
            Instance = instance,
            Status = statusCode,
            Title = title,
            Type = type.Text
        };
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        await HandleExceptionAsync(httpContext, exception, cancellationToken);

        return true;
    }
}
