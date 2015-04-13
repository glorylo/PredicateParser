﻿using System.Diagnostics;
using PredicateParser;

namespace Tests
{
    public static class ExpressionEvaluator
    {
        public static bool Evaluate<TSourceObj>(string expression, TSourceObj source)
        {
            var predicate = PredicateParser<TSourceObj>.Parse(expression);
            var compiledPredicate = predicate.Compile();
            Trace.WriteLine(predicate);
            return compiledPredicate(source);                    
        }
    }
}