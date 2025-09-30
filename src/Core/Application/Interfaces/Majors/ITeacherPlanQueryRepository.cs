using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Application.Interfaces.Subjects;

public interface ITeacherPlanQueryRepository : IQueryRepository<TeacherPlan>,
    IScopedDependency { }