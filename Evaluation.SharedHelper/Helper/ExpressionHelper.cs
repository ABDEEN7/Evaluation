using System.Linq.Expressions;

namespace Evaluation.SharedHelper.Helper;

public static class ExpressionHelper
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> first,
        Expression<Func<T, bool>> second)
    {
        var paramters = Expression.Parameter(typeof(T));
        var leftVisitor = new ReplaceExpressionVisitor(first.Parameters[0], paramters);
        var left = leftVisitor.Visit(first.Body);

        var rightVisitor = new ReplaceExpressionVisitor(second.Parameters[0], paramters);
        var rgiht = rightVisitor.Visit(second.Body);
        return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(left, rgiht), paramters);
    }
    private class ReplaceExpressionVisitor : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;
        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression? Visit(Expression? node)
        {
            return node == _oldValue ? _newValue : base.Visit(node);
        }
    }
}
