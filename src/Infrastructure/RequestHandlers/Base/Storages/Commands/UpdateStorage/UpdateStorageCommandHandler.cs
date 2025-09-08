using Mapster;
using SmartAttendance.Application.Base.Storage.Commands.UpdateStorage;
using SmartAttendance.Application.Interfaces.Storages;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Storages;

namespace SmartAttendance.RequestHandlers.Base.Storages.Commands.UpdateStorage;

public class UpdateStorageCommandHandler(
    IStorageCommandRepository storageCommandRepository,
    IStorageQueryRepository storageQueryRepository,
    ILogger<UpdateStorageCommandHandler> logger,
    IStringLocalizer<UpdateStorageCommandHandler> localizer
) : IRequestHandler<UpdateStorageCommand>
{
    public async Task Handle(UpdateStorageCommand request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            logger.LogWarning("UpdateStorageCommand request was null.");
            throw new InvalidNullInputException(nameof(request));
        }

        var existing = await storageQueryRepository.GetSingleAsync(cancellationToken, a => a.Id == request.Id);

        if (existing is null)
        {
            logger.LogWarning("Storage with ID {StorageId} not found.", request.Id);
            throw SmartAttendanceException.NotFound(localizer["Storage not found."]);
        }

        var storage = request.Adapt<Storage>();

        await storageCommandRepository.UpdateAsync(storage, cancellationToken);

        logger.LogInformation("Storage with ID {StorageId} updated successfully.", request.Id);
    }
}