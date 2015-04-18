using System;
using System.Linq.Expressions;
using PredicateParser.Extensions;

namespace PredicateParser
{
    public static class CompareExpression
    {
        private static Expression CompareTo(ExpressionType expressionType, Expression compare)
        {
            Expression zero = Expression.Constant(0);
            Expression expression;
            switch (expressionType)
            {
                case ExpressionType.Equal:
                    expression = Expression.Equal(compare, zero);
                    break;

                case ExpressionType.NotEqual:
                    expression = Expression.NotEqual(compare, zero);
                    break;

                case ExpressionType.GreaterThan:
                    expression = Expression.GreaterThan(compare, zero);
                    break;

                case ExpressionType.GreaterThanOrEqual:
                    expression = Expression.GreaterThanOrEqual(compare, zero);
                    break;

                case ExpressionType.LessThan:
                    expression = Expression.LessThan(compare, zero);
                    break;

                case ExpressionType.LessThanOrEqual:
                    expression = Expression.LessThanOrEqual(compare, zero);
                    break;

                default:
                    throw new ArgumentException("unexpected compare type: " + expressionType);

            }
            return expression;
        }

        /// <summary>produce comparison based on IComparable types</summary>
        public static Expression CompareTo(Expression lhs, Expression rhs, ExpressionType exprType)
        {
            if (lhs.Type.IsDynamic() || rhs.Type.IsDynamic())
                return DynamicOp.BinaryOpPredicate(lhs, rhs, exprType);

            lhs = ExpressionHelper.Coerce(lhs, rhs);
            rhs = ExpressionHelper.Coerce(rhs, lhs);
            var compareToMethod = lhs.Type.GetMethod("CompareTo", new[] { rhs.Type })
                                  ?? lhs.Type.GetMethod("CompareTo", new[] { typeof(object) });
            if (compareToMethod == null)
                throw new ArgumentException("unexpected IComparable types for instance: " + lhs.Type + " compared to " + rhs.Type);
            Expression compare = Expression.Call(lhs, compareToMethod, new[] { rhs });
            return CompareTo(exprType, compare);
        }

    }
}
