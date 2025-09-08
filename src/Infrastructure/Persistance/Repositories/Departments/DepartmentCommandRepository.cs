using SmartAttendance.Application.Interfaces.Departments;
using SmartAttendance.Domain.Departments;

namespace SmartAttendance.Persistence.Repositories.Departments;

public class DepartmentCommandRepository(
    WriteOnlyDbContext dbContext,
    ILogger<CommandRepository<Department>> logger
)
    : CommandRepository<Department>(dbContext, logger),
        IDepartmentCommandRepository;