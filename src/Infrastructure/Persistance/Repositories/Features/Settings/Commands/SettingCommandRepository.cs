using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shifty.Domain.Features.Setting;
using Shifty.Domain.Interfaces.Features.Settings;
using Shifty.Persistence.Db;
using Shifty.Persistence.Repositories.Common;

namespace Shifty.Persistence.Repositories.Features.Settings.Commands;

public class SettingCommandRepository(ReadOnlyDbContext dbContext , ILogger<Repository<Setting , ReadOnlyDbContext>> logger)
    : Repository<Setting , ReadOnlyDbContext>(dbContext , logger) , ISettingCommandRepository
{
    public override void Add(Setting entity , bool saveNow = true)
    {
        var setting = GetSingle();
        
        setting.Update(entity);
    }

    public override Task AddAsync(Setting entity , CancellationToken cancellationToken , bool saveNow = true)
    {
        return base.AddAsync(entity , cancellationToken , saveNow);
    }
}