using System;
using System.Threading;
using System.Threading.Tasks;
using Shifty.Common;
using Shifty.Domain.Features.Shifts;
using Shifty.Domain.Interfaces.Base;

namespace Shifty.Domain.Interfaces.Features.Shifts;

public interface IShiftQueryRepository : IScopedDependency , IRepository<Shift>
{
    Task<bool> Exist(Shift shift , CancellationToken cancellationToken);
}