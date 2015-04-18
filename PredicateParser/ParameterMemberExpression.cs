using System.Collections.Generic;
using System.Linq.Expressions;
using PredicateParser.Extensions;

namespace PredicateParser
{
    public static class ParameterMemberExpression
    {
        public static Expression Member(Expression lhs, string memberName)
        {
            if (!lhs.Type.IsDynamic())
                return Expression.PropertyOrField(lhs, memberName);
            return DynamicOp.GetMember(lhs, memberName);
        }

        public static Expression GetDictionaryValue(Expression lhs, string key)
        {
            lhs = ExpressionHelper.Coerce(lhs, typeof (IDictionary<string, object>));
            Expression keyExpr = Expression.Constant(key, typeof(string));
            return Expression.Property(lhs, "Item", keyExpr);            
        }


    }
}
