using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shifty.Domain.Common.BaseClasses;
using Shifty.Domain.Features.Setting;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;

namespace Shifty.Persistence.Repositories.Features.Settings.Queries;

public class SettingQueryRepository(WriteOnlyDbContext dbContext , ILogger<Repository<Setting , WriteOnlyDbContext>> logger)
    : Repository<Setting , WriteOnlyDbContext>(dbContext , logger) , ISettingQueriesRepository;