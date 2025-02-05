using System;
using Microsoft.EntityFrameworkCore;

namespace Shifty.Persistence.Db
{
    public class WriteOnlyDbContext(DbContextOptions<AppDbContext> options , Guid? userId) : AppDbContext(options , userId);
}