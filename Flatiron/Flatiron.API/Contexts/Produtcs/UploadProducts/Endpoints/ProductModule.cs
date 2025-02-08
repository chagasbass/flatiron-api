using Carter;
using Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;
using Flatiron.Extensions.EndpointModules;
using MediatR;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Endpoints;

public class ProductModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versioning = CarterVersionExtensions.VersionEndpoints(app);

        #region UploadingFiles

        app.MapPost("/v{version:apiVersion}/flatiron/products/uploads", [RequestSizeLimit(5_000_000), IgnoreAntiforgeryToken] async (IApiCustomResults customResults,
                             IMediator mediator,
                             [FromForm] UploadProductCommand command) =>
        {
            var commandResult = (CommandResult)await mediator.Send(command);

            return customResults.FormatApiResponse(commandResult);

        })
        .DisableAntiforgery()
        .Produces<CommandResult>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
        .WithName("Upload Product Files")
        .WithTags("Product Files Upload")
        .WithDescription(@" <ul>
          <li><b>File</b> - File for uploading (<b>only .xls or .xlsx extensions</b>)</li>
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

    }
}
