using Flatiron.API.Contexts.Produtcs.ListingProducts.Filters;
using Flatiron.API.Contexts.Produtcs.ListingProducts.Queries;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Queries;

public class ProductFilterQueryHandler(INotificationServices notificationServices,
                                  IProductRepository productRepository) : IRequestHandler<ProductFilterQuery, ICommandResult>
{
    public async Task<ICommandResult> Handle(ProductFilterQuery request, CancellationToken cancellationToken)
    {
        var notification = request.Validate();

        if (notification is not null)
        {
            notificationServices.AddNotification(notification, StatusCodeOperation.BadRequest);

            return new CommandResult(false, "Please, verify the input filter.");
        }

        var products = await productRepository.GetProductsByFilterAsync(request);

        if (!products.Any())
        {
            notificationServices.AddNotification(new Notification("get-products", "Sorry,the search does not find results."), StatusCodeOperation.NotFound);

            return new CommandResult(false, "Please, verify the input filter.");
        }

        var productQueries = new List<ProductQuery>();

        foreach (var product in products)
        {
            ProductQuery productQuery = product;
            productQueries.Add(productQuery);
        }

        notificationServices.AddStatusCode(StatusCodeOperation.OK);

        return new CommandResult(productQueries, true, "");
    }
}
