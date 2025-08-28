using Mapster;
using Shifty.Application.Base.Storage.Commands.CreateStorage;
using Shifty.Application.Interfaces.Storages;
using Shifty.Common.Exceptions;
using Shifty.Domain.Storages;

namespace Shifty.RequestHandlers.Base.Storages.Commands.CreateStorage;

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
            throw ShiftyException.InternalServerError(
                localizer["An unexpected error occurred while creating the storage."]);
        }
    }
}