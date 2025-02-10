namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Behaviors;

public class UploadProductFileBehaviors<TRequest, TResponse>(ILogServices logServices,
                                                             INotificationServices notificationServices)
: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
                                     where TResponse : ICommandResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        UploadProductCommand uploadProductCommand = request as UploadProductCommand;

        logServices.WriteMessage("Validating incoming file...");

        if (uploadProductCommand is null)
        {
            notificationServices.AddStatusCode(StatusCodeOperation.BusinessError);

            var commandResult = new CommandResult()
            {
                Data = new Notification("invalid-contract", "the api contract is invalid."),
                Success = false,
                Message = "There are errors on request."
            };

            var response = (TResponse)(object)commandResult;

            return await Task.FromResult(response);
        }

        uploadProductCommand.Validate();

        if (!uploadProductCommand.IsValid)
        {
            notificationServices.AddNotifications(uploadProductCommand.Notifications, StatusCodeOperation.BusinessError);

            var commandResult = new CommandResult()
            {
                Data = uploadProductCommand.Notifications.ToList(),
                Success = false,
                Message = "Please,verify your uploaded file."
            };

            var response = (TResponse)(object)commandResult;

            return await Task.FromResult(response);
        }

        return await next();
    }
}
