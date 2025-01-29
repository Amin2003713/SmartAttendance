using Microsoft.Extensions.Logging;
using Shifty.Common;
using Shifty.Domain.Features.Shifts;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;

namespace Shifty.Persistence.Repositories.Features.Shifts.Command;

public class ShiftCommandRepository(WriteOnlyDbContext dbContext , ILogger<Repository<Shift , WriteOnlyDbContext>> logger)
    : Repository<Shift , WriteOnlyDbContext>(dbContext , logger) , IScopedDependency;