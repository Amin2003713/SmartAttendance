using SmartAttendance.Application.Interfaces.Departments;
using SmartAttendance.Domain.Departments;

namespace SmartAttendance.Persistence.Repositories.Departments;

public class DepartmentQueryRepository(
    ReadOnlyDbContext dbContext,
    ILogger<QueryRepository<Department>> logger
)
    : QueryRepository<Department>(dbContext, logger),
        IDepartmentQueryRepository;