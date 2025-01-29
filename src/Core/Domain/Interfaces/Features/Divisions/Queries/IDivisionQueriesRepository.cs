using Shifty.Common;
using Shifty.Domain.Features.Divisions;
using Shifty.Domain.Interfaces.Base;

namespace Shifty.Domain.Interfaces.Features.Divisions.Queries;

public interface IDivisionQueriesRepository : IRepository<Division> , IScopedDependency;