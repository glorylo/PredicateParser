using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder;

namespace PredicateParser
{
    public static class DynamicOp
    {
        private static Expression UnaryOp(Expression lhs, ExpressionType expressionType)
        {
            var expArgs = new List<Expression>() { lhs };
            var binderM = Binder.UnaryOperation(CSharpBinderFlags.None, expressionType, lhs.Type, new []
		            {
			            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
			            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
		            });
            return Expression.Dynamic(binderM, typeof(object), expArgs);
        }

        public static Expression BinaryOp(Expression lhs, Expression rhs, ExpressionType expressionType)
        {
            var expArgs = new List<Expression>() { lhs, rhs };
            var binderM = Binder.BinaryOperation(CSharpBinderFlags.None, expressionType, lhs.Type, new []
		            {
			            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
			            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
		            });

            return Expression.Dynamic(binderM, typeof(object), expArgs);
        }

        public static Expression BinaryOpPredicate(Expression lhs, Expression rhs, ExpressionType expressionType)
        {
            return Expression.Convert(BinaryOp(lhs, rhs, expressionType), typeof(bool));
        }
    }
}
