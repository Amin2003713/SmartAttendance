using SmartAttendance.Application.Interfaces.Storages;

namespace SmartAttendance.Persistence.Repositories.Storage;

public class StorageCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Domain.Storages.Storage>> logger
)
    : CommandRepository<Domain.Storages.Storage>(dbContext, logger),
        IStorageCommandRepository { }