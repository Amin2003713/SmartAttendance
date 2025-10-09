using SmartAttendance.Application.Interfaces.Base;
using SmartAttendance.Common.Utilities.InjectionHelpers;
using SmartAttendance.Domain.Features.PlanEnrollments;
using SmartAttendance.Domain.Features.Plans;

namespace SmartAttendance.Application.Interfaces.Plans;

public interface IPlanQueryRepository : IQueryRepository<Plan>,
    IScopedDependency { }

public interface IPlanEnrollmentQueryRepository : IQueryRepository<PlanEnrollment>,
    IScopedDependency { }