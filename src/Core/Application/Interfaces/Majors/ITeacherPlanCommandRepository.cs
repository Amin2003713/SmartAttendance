using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Majors;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Application.Interfaces.Majors;

public interface ITeacherPlanCommandRepository : ICommandRepository<TeacherPlan>,
    IScopedDependency { }

public interface IMajorCommandRepository : ICommandRepository<Major>,
    IScopedDependency { }

public interface IMajorSubjectCommandRepository : ICommandRepository<MajorSubject>,
    IScopedDependency { }