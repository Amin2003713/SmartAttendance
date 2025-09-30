using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.Subjects;

namespace SmartAttendance.Application.Interfaces.Majors;

public interface ISubjectPlansCommandRepository : ICommandRepository<SubjectPlans>,
    IScopedDependency { }