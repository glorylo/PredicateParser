using System;
using System.Linq.Expressions;
using PredicateParser.Extensions;

namespace PredicateParser
{
    public static class MathExpression
    {
        public static Expression MathOp(Expression lhs, Expression rhs, ExpressionType expressionType)
        {
            var coerceLeft = ExpressionHelper.Coerce(lhs, rhs);
            var coerceRight = ExpressionHelper.Coerce(rhs, lhs);

            if (lhs.Type.IsDynamic() || rhs.Type.IsDynamic())
                DynamicOp.BinaryOp(coerceLeft, coerceRight, expressionType);

            Expression expression;
            switch (expressionType)
            {
                case ExpressionType.Add:
                    expression = Expression.Add(coerceLeft, coerceRight);
                    break;

                case ExpressionType.Subtract:
                    expression = Expression.Subtract(coerceLeft, coerceRight);
                    break;

                case ExpressionType.Multiply:
                    expression = Expression.Multiply(coerceLeft, coerceRight);
                    break;

                case ExpressionType.Divide:
                    expression = Expression.Divide(coerceLeft, coerceRight);
                    break;

                case ExpressionType.Modulo:
                    expression = Expression.Modulo(coerceLeft, coerceRight);
                    break;
                default:
                    throw new ArgumentException("unknown math type:  " + expressionType);
            }
            return expression;
        }

        public static Expression Negate(Expression lhs)
        {
            if (lhs.Type.IsDynamic())
                return DynamicOp.UnaryOp(lhs, ExpressionType.Negate);
            return Expression.Negate(lhs);
        }

    }
}
