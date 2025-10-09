using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Application.Interfaces.Majors;

public interface ITeacherPlanQueryRepository : IQueryRepository<TeacherPlan>,
    IScopedDependency { }

public interface IMajorQueryRepository : IQueryRepository<Major>,
    IScopedDependency { }

public interface IMajorSubjectQueryRepository : IQueryRepository<MajorSubject>,
    IScopedDependency { }