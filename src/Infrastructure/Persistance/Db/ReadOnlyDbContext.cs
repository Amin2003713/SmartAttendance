using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shifty.Common.Exceptions;
using Shifty.Resources.Messages;

namespace Shifty.Persistence.Db
{
    public class ReadOnlyDbContext(DbContextOptions<AppDbContext> options) : AppDbContext(options);
}