using Shifty.Application.Interfaces.Departments;
using Shifty.Domain.Departments;

namespace Shifty.Persistence.Repositories.Departments;

public class DepartmentQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Department>> logger
)
    : QueryRepository<Department>(dbContext, logger),
        IDepartmentQueryRepository;