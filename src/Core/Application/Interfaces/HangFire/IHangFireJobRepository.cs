using System.Linq.Expressions;
using SmartAttendance.Common.Utilities.InjectionHelpers;

namespace SmartAttendance.Application.Interfaces.HangFire;

public interface IHangFireJobRepository : IScopedDependency
{
    string AddFireAndForgetJob(Expression<Action> methodCall);
    string AddDelayedJob(Expression<Action> methodCall, TimeSpan delay);
    void   AddOrUpdateRecurringJob(string recurringJobId, Expression<Action> methodCall, string cronExpression);
    void   RemoveRecurringJob(string recurringJobId);
    void   RequeueJob(string jobId);
    void   DeleteJob(string jobId);
}