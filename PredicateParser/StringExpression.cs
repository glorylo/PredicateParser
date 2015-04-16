using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PredicateParser
{
    public static class StringExpression
    {
        private static readonly Type _string = typeof (string);

        private static MethodInfo GetMethodInfo(string name, Type[] types) { return typeof(string).GetMethod(name, types); }
        
        public static Expression StartsWith(Expression lhs, Expression rhs)
        {
            return Expression.Call(lhs, GetMethodInfo("StartsWith", new[] { typeof(string) }), new[] { rhs });
        }

        public static Expression EndsWith(Expression lhs, Expression rhs)
        {
            return Expression.Call(lhs, GetMethodInfo("EndsWith", new[] {typeof (string)}), new[] {rhs});
        }

        public static Expression Containing(Expression lhs, Expression rhs)
        {
            return Expression.Call(lhs, GetMethodInfo("Contains", new[] {typeof (string)}), new[] {rhs});
        }

        public static Expression Equals(Expression lhs, Expression rhs)
        {
            return Expression.Call(lhs, GetMethodInfo("Equals", new[] {typeof (string)}), new[] {rhs});
        }

        public static Expression Matching(Expression lhs, Expression rhs)
        {
            var matchMethod = typeof(Regex).GetMethod("Match", new[] { typeof(string), typeof(string) });
            var args = new[] { lhs }.Concat(new [] {rhs});
            Expression callExp = Expression.Call(matchMethod, args);
            var result = Expression.Parameter(typeof(Match), "result");
            var block = Expression.Block(
                         new[] { result },
                         Expression.Assign(result, callExp),
                         Expression.PropertyOrField(result, "Success")
            );
            return block;   
        }

    }
}
