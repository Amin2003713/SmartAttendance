using Shifty.Application.Interfaces.Base;
using Shifty.Common.Utilities.InjectionHelpers;
using Shifty.Domain.Departments;

namespace Shifty.Application.Interfaces.Departments;

public interface IDepartmentCommandRepository : ICommandRepository<Department>,
    IScopedDependency;