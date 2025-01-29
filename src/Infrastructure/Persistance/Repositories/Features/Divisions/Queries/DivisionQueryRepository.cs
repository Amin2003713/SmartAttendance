using Microsoft.Extensions.Logging;
using Shifty.Domain.Features.Divisions;
using Shifty.Domain.Interfaces.Features.Divisions.Queries;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;

namespace Shifty.Persistence.Repositories.Features.Divisions.Queries;

public class DivisionQueryRepository(ReadOnlyDbContext dbContext , ILogger<Repository<Division , ReadOnlyDbContext>> logger)
    : Repository<Division , ReadOnlyDbContext>(dbContext , logger) , IDivisionQueriesRepository;