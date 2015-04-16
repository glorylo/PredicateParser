using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

namespace PredicateParser
{
    public static class DynamicOp
    {
        public static Expression UnaryOp(Expression lhs, ExpressionType expressionType)
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
            lhs = ExpressionHelper.Coerce(lhs, typeof (object));
            rhs = ExpressionHelper.Coerce(rhs, typeof(object));
            return Expression.Convert(BinaryOp(lhs, rhs, expressionType), typeof(bool));
        }

        public static Expression GetMember(Expression lhs, string memberName)
        {
            var binder = Binder.GetMember(
                CSharpBinderFlags.None,
                memberName,
                typeof(ExpandoObject),
                new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }
                );
            var member = Expression.Dynamic(binder, typeof(object), lhs);
#if DEBUG
            Debug.WriteLine(member);
#endif
            return member;            
        }
    }
}
