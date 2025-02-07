namespace Flatiron.Extensions.Middlewares;

public static class MiddlewareExtensions
{
    public static IServiceCollection AddGlobalExceptionHandlerMiddleware(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandlerMiddleware>();

        services.AddProblemDetails();

        return services;
    }
}
