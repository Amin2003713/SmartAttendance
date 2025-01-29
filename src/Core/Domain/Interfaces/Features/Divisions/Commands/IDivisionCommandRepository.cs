using Shifty.Common;
using Shifty.Domain.Features.Divisions;
using Shifty.Domain.Interfaces.Base;

namespace Shifty.Domain.Interfaces.Features.Divisions.Commands;

public interface IDivisionCommandRepository : IRepository<Division> , IScopedDependency;