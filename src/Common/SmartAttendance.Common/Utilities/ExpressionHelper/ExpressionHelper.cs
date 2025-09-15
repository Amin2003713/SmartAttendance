using SmartAttendance.Common.Utilities.MongoHelpers;

namespace SmartAttendance.Common.Utilities.ExpressionHelper;

public static class ExpressionHelper
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>>      second)
    {
        var parameter  = first.Parameters[0];
        var visitor    = new ReplaceParameterVisitor(second.Parameters[0], parameter);
        var secondBody = visitor.Visit(second.Body)!;

        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(first.Body, secondBody),
            parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>>      second)
    {
        var parameter  = first.Parameters[0];
        var visitor    = new ReplaceParameterVisitor(second.Parameters[0], parameter);
        var secondBody = visitor.Visit(second.Body)!;

        return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(first.Body, secondBody),
            parameter);
    }

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression)
    {
        var parameter = expression.Parameters[0];
        var body      = Expression.Not(expression.Body);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public static Expression<Func<T, bool>> True<T>()
    {
        return _ => true;
    }

    public static Expression<Func<T, bool>> False<T>()
    {
        return _ => false;
    }

    public static Expression<Func<T, bool>> AndIf<T>(
        this Expression<Func<T, bool>> expr,
        bool                           condition,
        Expression<Func<T, bool>>      next)
    {
        return condition ? expr.And(next) : expr;
    }

    public static Expression<Func<T, bool>> OrIf<T>(
        this Expression<Func<T, bool>> expr,
        bool                           condition,
        Expression<Func<T, bool>>      next)
    {
        return condition ? expr.Or(next) : expr;
    }

    public static Expression<Func<T, bool>> Combine<T>(
        this IEnumerable<Expression<Func<T, bool>>>    expressions,
        Func<Expression, Expression, BinaryExpression> combiner)
    {
        var exprList = expressions.ToList();

        if (!exprList.Any())
            return True<T>();

        var parameter = exprList[0].Parameters[0];
        var body      = exprList[0].Body;

        for (var i = 1; i < exprList.Count; i++)
        {
            var visitor  = new ReplaceParameterVisitor(exprList[i].Parameters[0], parameter);
            var nextBody = visitor.Visit(exprList[i].Body)!;
            body = combiner(body!, nextBody);
        }

        return Expression.Lambda<Func<T, bool>>(body!, parameter);
    }
}