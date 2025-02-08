using Flatiron.API.Contexts.Produtcs.UploadProducts.Commands;
using Flatiron.API.Contexts.Produtcs.UploadProducts.DomainServices;
using Flatiron.Extensions.Shared.CustomsLogs.Services;
using Flunt.Notifications;
using MediatR;

namespace Flatiron.API.Contexts.Produtcs.UploadProducts.Behaviors;

public class UploadProductFileBehaviors<TRequest, TResponse>(ILogServices logServices,
                                                             INotificationServices notificationServices,
                                                             IFileValidatorDomainServices fileValidatorDomainServices)
: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
                                     where TResponse : ICommandResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        UploadProductCommand uploadProductCommand = request as UploadProductCommand;

        logServices.WriteMessage("validating incoming  file");

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

        fileValidatorDomainServices.ValidateUploadFileAsync(uploadProductCommand);

        if (notificationServices.HasNotifications())
        {
            notificationServices.AddStatusCode(StatusCodeOperation.BusinessError);

            var commandResult = new CommandResult()
            {
                Data = uploadProductCommand.Notifications.ToList(),
                Success = false,
                Message = "Verify your uploaded file."
            };

            var response = (TResponse)(object)commandResult;

            return await Task.FromResult(response);
        }

        return await next();

    }
}
