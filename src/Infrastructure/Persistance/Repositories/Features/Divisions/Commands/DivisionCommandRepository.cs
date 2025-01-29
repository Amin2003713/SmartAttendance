using Microsoft.Extensions.Logging;
using Shifty.Domain.Features.Divisions;
using Shifty.Domain.Interfaces.Features.Divisions.Commands;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;

namespace Shifty.Persistence.Repositories.Features.Divisions.Commands;

public class DivisionCommandRepository(WriteOnlyDbContext dbContext , ILogger<Repository<Division , WriteOnlyDbContext>> logger)
    : Repository<Division , WriteOnlyDbContext>(dbContext , logger) , IDivisionCommandRepository;