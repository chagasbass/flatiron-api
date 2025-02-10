namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Endpoints;

public class ProductUploadModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versioning = CarterVersionExtensions.VersionEndpoints(app);
        var postEndpoint = "/flatiron/products";

        #region UploadingFiles

        app.MapPost("/v{version:apiVersion}/flatiron/products", [RequestSizeLimit(5_000_000), IgnoreAntiforgeryToken] async (IApiCustomResults customResults,
                             IMediator mediator,
                             [FromForm] UploadProductCommand command) =>
        {
            var commandResult = (CommandResult)await mediator.Send(command);

            return customResults.FormatApiResponse(commandResult, postEndpoint);
        })
        .DisableAntiforgery()
        .Produces<CommandResult>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
        .WithName("Upload Product Files")
        .WithTags("Products")
        .WithDescription(@" <ul>
          <li><b>File</b> - File for uploading (<b>only .xls,.xlsx and .csv extensions</b>)</li>
           </ul>
             &#09;The response always will be a CommandResult pattern object <b>CommandResult</b>: <br/>
                                    {
                                      ""<b>success</b>"": 'Show if the request was succesfull or not',
                                      ""<b>message</b>"": 'Message about request status',
                                      ""<b>data</b>"": 'Content of response( If has errors, it will be an object type <b>ProblemDetails</b>)'
                                     }"
)
        .WithSummary("Endpoint responsable for uploading Information Product files")
        .WithOpenApi()
        .WithApiVersionSet(versioning)
        .MapToApiVersion(1);

        #endregion

        #region Get Product Information


        app.MapGet("/v{version:apiVersion}/flatiron/products", async (IApiCustomResults customResults,
                             IMediator mediator,
                             [AsParameters] ProductFilterQuery productFilter) =>
        {
            var commandResult = (CommandResult)await mediator.Send(productFilter);

            return customResults.FormatApiResponse(commandResult);

        })
        .Produces<CommandResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
        .WithName("Products")
        .WithTags("Products")
        .WithDescription(@" <ul>
                          <li><b>Name</b> - The name of the product (<b>not required</b>)</li>
                          <li><b>Price</b> - The price of the product (<b>not required</b>)</li>
                          <li><b>Expiration</b> - The Expiration Date (<b>not required</b>)</li>
                          <li><b>Sort</b> - The properties for sorts the search.(Only name,price and expiration.)(<b>not required</b>)</li>
                           </ul>
                             &#09;The response always will be a CommandResult pattern object <b>CommandResult</b>: <br/>
                                                    {
                                                      ""<b>success</b>"": 'Show if the request was succesfull or not',
                                                      ""<b>message</b>"": 'Message about request status',
                                                      ""<b>data</b>"": 'Content of response( If has errors, it will be an object type <b>ProblemDetails</b>)'
                                                     }"
)
        .WithSummary("Endpoint responsable for retrieve product data and his currencies")
        .WithOpenApi()
        .WithApiVersionSet(versioning)
        .MapToApiVersion(1);

        #endregion

    }
}
