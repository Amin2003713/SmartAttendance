using Mapster;
using Shifty.Application.Base.Storage.Commands.DeleteStorage;
using Shifty.Application.Interfaces.Storages;
using Shifty.Common.Exceptions;
using Shifty.Domain.Storages;

namespace Shifty.RequestHandlers.Base.Storages.Commands.DeleteStorage;

public class DeleteStorageCommandHandler(
    IStorageCommandRepository storageCommandRepository,
    IStorageQueryRepository storageQueryRepository,
    ILogger<DeleteStorageCommandHandler> logger,
    IStringLocalizer<DeleteStorageCommandHandler> localizer
)
    : IRequestHandler<DeleteStorageCommand>
{
    public async Task Handle(DeleteStorageCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("DeleteStorageCommand request was null.");
            throw new InvalidNullInputException(nameof(request));
        }

        var storage = await storageQueryRepository.GetSingleAsync(cancellationToken, a => a.Id == request.Id);

        if (storage is null)
        {
            logger.LogWarning("Storage with ID {Id} not found.", request.Id);
            throw ShiftyException.NotFound(localizer["Storage not found."]);
        }

        storage = request.Adapt<Storage>();

        await storageCommandRepository.DeleteAsync(storage, cancellationToken);
        logger.LogInformation("Storage with ID {Id} deleted successfully.", request.Id);
    }
}