using Microsoft.Extensions.Logging;
using Shifty.Common;
using Shifty.Domain.Features.Shifts;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;

namespace Shifty.Persistence.Repositories.Features.Shifts.Query;

public class ShiftQueryRepository(ReadOnlyDbContext dbContext , ILogger<Repository<Shift , ReadOnlyDbContext>> logger)
    : Repository<Shift , ReadOnlyDbContext>(dbContext , logger) , IScopedDependency;