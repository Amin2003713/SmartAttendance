using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.PlanEnrollments;

namespace SmartAttendance.Application.Interfaces.Plans;

public interface IPlanEnrollmentQueryRepository : IQueryRepository<PlanEnrollment>,
    IScopedDependency { }