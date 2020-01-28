using System;
using System.Linq;
using System.Linq.Expressions;

namespace PredicateParser
{
    public static class ExpressionHelper
    {
        private static readonly Type[] _prom = 
          { typeof(decimal), typeof(double), typeof(float), typeof(ulong), typeof(long), typeof(uint),
            typeof(int), typeof(ushort), typeof(char), typeof(short), typeof(byte), typeof(sbyte) };
        
        /// <summary>enforce the type on the expression (by a cast) if not already of that type</summary>
        public static Expression Coerce(Expression expr, Type type, bool useToString = false)
        {
            if (useToString && type == typeof(string) && expr.Type != type)
            {
                return Expression.Call(expr, typeof(object).GetMethod("ToString"));
            }
            return expr.Type == type ? expr : Expression.Convert(expr, type);
        }

        /// <summary>casts if needed the expr to the "largest" type of both arguments</summary>
        public static Expression Coerce(Expression expr, Expression sibling)
        {
            if (expr.Type != sibling.Type)
            {
                Type maxType = MaxType(expr.Type, sibling.Type);
                if (maxType != expr.Type) expr = Expression.Convert(expr, maxType);
            }
            return expr;
        }

        /// <summary>returns the first if both are same, or the largest type of both (or the first)</summary>
        public static Type MaxType(Type a, Type b)
        {
            return a == b ? a : (_prom.FirstOrDefault(t => t == a || t == b) ?? a);
        }



    }
}
