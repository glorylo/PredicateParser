using System;
using System.Linq.Expressions;

namespace PredicateParser
{
    public static class BooleanExpression
    {
        private static readonly Type _bool = typeof(bool);        

        public static Expression Or(Expression lhs, Expression rhs)
        {
            return Expression.OrElse(ExpressionHelper.Coerce(lhs, _bool), ExpressionHelper.Coerce(rhs, _bool));
        }

        public static Expression And(Expression lhs, Expression rhs)
        {
            return Expression.AndAlso(ExpressionHelper.Coerce(lhs, _bool), ExpressionHelper.Coerce(rhs, _bool));
        }

        public static Expression Not(Expression rhs)
        {
            return Expression.Not(ExpressionHelper.Coerce(rhs, _bool));
        }


    }
}
