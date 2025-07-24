using System.Linq.Expressions;
using Shifty.Common.InjectionHelpers;

namespace Shifty.Application.Commons.HangFire;

public interface IHangFireJobRepository : IScopedDependency
{
    string AddFireAndForgetJob(Expression<Action> methodCall);
    string AddDelayedJob(Expression<Action> methodCall, TimeSpan delay);
    void   AddOrUpdateRecurringJob(string recurringJobId, Expression<Action> methodCall, string cronExpression);
    void   RemoveRecurringJob(string recurringJobId);
    void   RequeueJob(string jobId);
    void   DeleteJob(string jobId);
}