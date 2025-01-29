using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shifty.Common;
using Shifty.Domain.Features.Shifts;
using Shifty.Domain.Interfaces.Features.Shifts;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;

namespace Shifty.Persistence.Repositories.Features.Shifts.Query;

public class ShiftQueryRepository(ReadOnlyDbContext dbContext , ILogger<Repository<Shift , ReadOnlyDbContext>> logger)
    : Repository<Shift , ReadOnlyDbContext>(dbContext , logger) , IShiftQueryRepository
{
    public async Task<bool> Exist(Shift shift , CancellationToken cancellationToken)
        => await TableNoTracking
            .AnyAsync(a=>a.Leave == shift.Leave && a.Arrive == shift.Arrive && a.GraceEarlyLeave == shift.GraceEarlyLeave && a.GraceLateArrival == shift.GraceLateArrival , cancellationToken);
}