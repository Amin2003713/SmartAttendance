using Shifty.Application.Interfaces.Departments;
using Shifty.Domain.Departments;

namespace Shifty.Persistence.Repositories.Departments;

public class DepartmentCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Department>> logger
)
    : CommandRepository<Department>(dbContext, logger),
        IDepartmentCommandRepository;