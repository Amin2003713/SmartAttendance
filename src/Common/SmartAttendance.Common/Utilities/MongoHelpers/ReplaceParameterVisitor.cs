namespace SmartAttendance.Common.Utilities.MongoHelpers;

public class ReplaceParameterVisitor(
    ParameterExpression from,
    ParameterExpression to
) : ExpressionVisitor
{
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == from
            ? to
            : base.VisitParameter(node);
    }
}