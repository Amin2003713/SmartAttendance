using Shifty.Application.Interfaces.Storages;

namespace Shifty.Persistence.Repositories.Storage;

public class StorageCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Domain.Storages.Storage>> logger
)
    : CommandRepository<Domain.Storages.Storage>(dbContext, logger),
        IStorageCommandRepository { }