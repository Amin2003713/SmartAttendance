using Mapster;
using SmartAttendance.Application.Base.Storage.Commands.CreateStorage;
using SmartAttendance.Application.Interfaces.Storages;
using SmartAttendance.Common.Exceptions;
using SmartAttendance.Domain.Storages;

namespace SmartAttendance.RequestHandlers.Base.Storages.Commands.CreateStorage;

public class CreateStorageCommandHandler(
    IStorageCommandRepository storageCommandRepository,
    ILogger<CreateStorageCommandHandler> logger,
    IStringLocalizer<CreateStorageCommandHandler> localizer
) : IRequestHandler<CreateStorageCommand>
{
    public async Task Handle(CreateStorageCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request == null)
                throw new InvalidNullInputException(nameof(request));

            var storage = request.Adapt<Storage>();
            await storageCommandRepository.AddAsync(storage, cancellationToken);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while creating storage.");
            throw SmartAttendanceException.InternalServerError(
                localizer["An unexpected error occurred while creating the storage."]);
        }
    }
}