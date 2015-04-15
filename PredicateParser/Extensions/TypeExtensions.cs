﻿using System;
using System.Dynamic;
using System.Linq;

namespace PredicateParser.Extensions
{
    internal static class TypeExtensions
    {
        public static bool IsDynamic(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IDynamicMetaObjectProvider)) ||
                   type == typeof(Object);
        }
    }
}
