using System.Diagnostics;
using PredicateParser;

namespace PredicateParser.Tests
{
    public static class ExpressionEvaluator
    {
        public static bool Evaluate<TSourceObj>(string expression, TSourceObj source)
        {
            var predicate = PredicateParser<TSourceObj>.Parse(expression);
            var compiledPredicate = predicate.Compile();
            Trace.WriteLine("Expression:  " + expression);
            Trace.WriteLine(predicate);
            return compiledPredicate(source);                    
        }
    }
}
