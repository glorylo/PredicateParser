using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CSharp.RuntimeBinder;
using PredicateParser.Extensions;
using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

// Forked version of PredicateParser originally by Andreas Gieriet
// See this Article:  http://www.codeproject.com/Articles/355513/Invent-your-own-Dynamic-LINQ-parser
namespace PredicateParser
  {
      public abstract class PredicateParser
      {
          #region built-in reserved words

          protected static readonly string[] ReservedWords = {"StartsWith?", "EndsWith?", "Containing?", "Matching?", "Equals?"};

          protected static MethodInfo GetMethodInfo(string name, Type[] types) { return typeof(string).GetMethod(name, types); }

          protected static readonly IDictionary<string, Func<Expression, IEnumerable<Expression>, Expression>> _builtInReservedWords = 
              new Dictionary<string, Func<Expression, IEnumerable<Expression>, Expression>>
              {
              { "StartsWith?", (instance, args) => Expression.Call(instance, GetMethodInfo("StartsWith", new [] { typeof(string) }), args) },
              { "EndsWith?", (instance, args) => Expression.Call(instance, GetMethodInfo("EndsWith", new [] { typeof(string) }), args)  },
              { "Containing?", (instance, args) => Expression.Call(instance, GetMethodInfo("Contains" , new [] { typeof(string)}), args) },
              { "Matching?", (str, pattern) =>
              {
                    var matchMethod = typeof (Regex).GetMethod("Match", new[] {typeof (string), typeof (string)});
                    var args = new [] {str}.Concat(pattern);
                    Expression callExp = Expression.Call(matchMethod, args);
                    var result = Expression.Parameter(typeof(Match), "result");
                    var block = Expression.Block(
                                 new[] { result },                             
                                 Expression.Assign(result, callExp),
                                 Expression.PropertyOrField(result, "Success")                         
                    );
                    return block;                  
                }},
              { "Equals?", (instance, args) => Expression.Call(instance, GetMethodInfo("Equals" , new [] { typeof(string)}), args) },
          }; 

          #endregion
          #region scanner

          protected static readonly string[] Operators = { "||", "&&", "==", "!=", "<=", ">=", "+", "-", "/", "*"};
          protected static readonly string[] Booleans = { "true", "false" };
          protected static readonly string Null = "null";

          /// <summary>tokenizer pattern: Optional-SpaceS...Token...Optional-Spaces</summary>
          private static readonly string _pattern = @"\s*(" + string.Join("|", new []
          {              
              string.Join("|", ReservedWords.Select(Regex.Escape)), // reserved words                   
              // operators and punctuation that are longer than one char: longest first
              string.Join("|", Booleans.Select(Regex.Escape)),   // booleans
              string.Join("|", Operators.Select(Regex.Escape)),  // operators
              @"""(?:\\.|[^""])*""", // string
              @"\d+(?:\.\d+)?", // number with optional decimal part
              @"\w+", // word
              @"\[(?:\s*)((?:\w+\s*)+)(?:\s*)\]", // indexer for square brackets
              @"\S", // other 1-char tokens (or eat up one character in case of an error)
          }) + @")\s*";

          /// <summary>get 1st char of current token (or a Space if no 1st char is obtained)</summary>
          private char Ch { get { return string.IsNullOrEmpty(Curr) ? ' ' : Curr[0]; } }
          /// <summary>move one token ahead</summary><returns>true = moved ahead, false = end of stream</returns>
          private bool Move() { return _tokens.MoveNext(); }
          /// <summary>the token stream implemwented as IEnumerator&lt;string&gt;</summary>
          private IEnumerator<string> _tokens;
          /// <summary>constructs the scanner for the given input string</summary>
          protected PredicateParser(string s)
          {
              _tokens = Regex.Matches(s, _pattern, RegexOptions.Compiled).Cast<Match>()
                        .Select(m => m.Groups[1].Value).GetEnumerator();
              Move();
          }
          protected bool IsBool { get { return (Curr == "true") || (Curr == "false");  } }
          protected bool IsNumber { get { return char.IsNumber(Ch); } }
          protected bool IsDouble { get { return IsNumber && Curr.Contains('.'); } }
          protected bool IsString { get { return Ch == '"'; } }
          protected bool IsIndexer { get { return Ch == '['; }}
          protected bool IsIdent { get { char c = Ch; return char.IsLower(c) || char.IsUpper(c) || c == '_'; } }
          /// <summary>throw an argument exception</summary>
          protected static void Abort(string msg) { throw new ArgumentException("Parse Error: " + (msg ?? "unknown error")); }
          /// <summary>get the current item of the stream or an empty string after the end</summary>
          protected string Curr { get { return _tokens.Current ?? string.Empty; }}
          /// <summary>get current and move to the next token (error if at end of stream)</summary>
          protected string CurrAndNext { get { string s = Curr; if (!Move()) Abort("data expected"); return s; } }
          /// <summary>get current and move to the next token if available</summary>
          protected string CurrOptNext { get { string s = Curr; Move(); return s; } }
          /// <summary>moves forward if current token matches and returns that (next token must exist)</summary>
          protected string CurrOpAndNext(params string[] ops)
          {
              string s = ops.Contains(Curr) ? Curr : null;
              if (s != null && !Move()) Abort("data expected");
              return s;
          }
          #endregion
      }

      public class PredicateParser<TData>: PredicateParser
      {
          #region code generator          
          private static readonly Type _bool = typeof(bool);
          private static readonly Type _string = typeof(string);
          private static readonly Type _object = typeof (object);

          private static Expression CompareTo(ExpressionType expressionType, Expression compare)
          {
              Expression zero = Expression.Constant(0);
              Expression expr = null; 
              switch (expressionType)
              {
                  case ExpressionType.Equal:
                      expr = Expression.Equal(compare, zero);
                      break;

                  case ExpressionType.NotEqual:
                      expr = Expression.NotEqual(compare, zero);
                      break;

                  case ExpressionType.GreaterThan:
                      expr = Expression.GreaterThan(compare, zero);
                      break;

                  case ExpressionType.GreaterThanOrEqual:
                      expr = Expression.GreaterThanOrEqual(compare, zero);
                      break;

                  case ExpressionType.LessThan:
                      expr = Expression.LessThan(compare, zero);
                      break;

                  case ExpressionType.LessThanOrEqual:
                      expr = Expression.LessThanOrEqual(compare, zero);
                      break;

                  default:  
                       Abort("unexpected compare type: " + expressionType);
                       break;
              }
              return expr;
          }

          /// <summary>produce comparison based on IComparable types</summary>
          //private static Expression CompareToExpression(Expression lhs, Expression rhs, Func<Expression, Expression> rel)
          private Expression CompareToExpression(Expression lhs, Expression rhs, ExpressionType exprType)
          {
              if (lhs.Type.IsDynamic() || rhs.Type.IsDynamic())
                  return DynamicOp.BinaryOpPredicate(ExpressionHelper.Coerce(lhs, _object), ExpressionHelper.Coerce(rhs, _object), exprType);

              lhs = ExpressionHelper.Coerce(lhs, rhs);
              rhs = ExpressionHelper.Coerce(rhs, lhs);
              var compareToMethod = lhs.Type.GetMethod("CompareTo", new[] {rhs.Type})
                                    ?? lhs.Type.GetMethod("CompareTo", new[] {typeof (object)});
              if (compareToMethod == null)
                  Abort("unexpected IComparable types for instance: " + lhs.Type + " compared to " + rhs.Type);
              Expression compare = Expression.Call(lhs, compareToMethod, new []{ rhs });
              return CompareTo(exprType, compare);
          }


          private static Expression MathExpression(Expression lhs, Expression rhs, ExpressionType expressionType)
          {
              var coerceLeft = ExpressionHelper.Coerce(lhs, rhs);
              var coerceRight = ExpressionHelper.Coerce(rhs, lhs);

              if (lhs.Type.IsDynamic() || rhs.Type.IsDynamic())
                  DynamicOp.BinaryOp(coerceLeft, coerceRight, expressionType);

              Expression exp = null;
              switch (expressionType)
              {
                  case ExpressionType.Add:
                      exp = Expression.Add(coerceLeft, coerceRight);
                      break;

                  case ExpressionType.Subtract:
                      exp = Expression.Subtract(coerceLeft, coerceRight);
                      break;

                  case ExpressionType.Multiply:
                      exp = Expression.Multiply(coerceLeft, coerceRight);
                      break;

                  case ExpressionType.Divide:
                      exp = Expression.Divide(coerceLeft, coerceRight);
                      break;

                  case ExpressionType.Modulo:
                      exp = Expression.Modulo(coerceLeft, coerceRight);
                      break;
                  default:
                      Abort("unknown math type:  " + expressionType);
                      break;
              }
              return exp;
          }

          /// <summary>
          /// Code generation of binary and unary epressions, utilizing type coercion where needed
          /// </summary>
          private readonly Dictionary<string, Func<Expression, Expression, Expression>> _binaryOperators; 

          /// <summary>
          /// Creates a expression for the reserved words:  StartsWith?, EndsWith?, etc.
          /// </summary>
          /// <param name="reservedWord">The reserved word</param>
          /// <param name="lhs">The expression on the left hand side</param>
          /// <param name="rhs">The expression on the right hand side</param>
          /// <returns></returns>
          private static Expression ReservedWordPredicate(string reservedWord, Expression lhs, Expression rhs)
          {
              if (!ReservedWords.Contains(reservedWord))
                  Abort("unknown reserved word:  " + reservedWord);

              if (lhs.Type != typeof(string) || rhs.Type != typeof(string))
                  Abort("expecting string type for predicate");

              return _builtInReservedWords[reservedWord](lhs, new[] {rhs});
          }

          private static readonly Dictionary<string, Func<Expression, Expression>> _unOp =
              new Dictionary<string, Func<Expression, Expression>>()
          {
              { "!", a=>Expression.Not(ExpressionHelper.Coerce(a, _bool)) },
              { "-", Expression.Negate },
          };

          /// <summary>create a constant of a value</summary>
          private static ConstantExpression Const(object v) { return Expression.Constant(v); }

          /// <summary>create lambda parameter field or property access.</summary>
          private Expression ParameterMember( string s)
          {
              if (!typeof(TData).IsDynamic())
                  return Expression.PropertyOrField(_param, s);

              var binder = Binder.GetMember(
                  CSharpBinderFlags.None,
                  s,
                  typeof(ExpandoObject),
                  new[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }
                  );
              var member = Expression.Dynamic(binder, typeof(object), _param);
#if DEBUG
              Debug.WriteLine(member);
#endif
              return member;
          }

          /// <summary>create lambda expression</summary>
          private Expression<Func<TData, bool>> Lambda(Expression expr) { return Expression.Lambda<Func<TData, bool>>(expr, _param); }
          /// <summary>the lambda's parameter (all names are members of this)</summary>
          private readonly ParameterExpression _param = Expression.Parameter(typeof(TData), "_p_");
          #endregion
          #region parser

          /// <summary>initialize the parser (and thus, the scanner)</summary>
          private PredicateParser(string s) : base(s)
          {
              _binaryOperators = new Dictionary<string, Func<Expression, Expression, Expression>>
              {
                 { "||", (a,b)=>Expression.OrElse(ExpressionHelper.Coerce(a, _bool), ExpressionHelper.Coerce(b, _bool)) },
                 { "&&", (a,b)=>Expression.AndAlso(ExpressionHelper.Coerce(a, _bool), ExpressionHelper.Coerce(b, _bool)) },
                 { "==", (a,b)=>CompareToExpression(a, b, ExpressionType.Equal) },
                 { "!=", (a,b)=>CompareToExpression(a, b, ExpressionType.NotEqual) },
                 { "<",  (a,b)=>CompareToExpression(a, b, ExpressionType.LessThan) },
                 { "<=", (a,b)=>CompareToExpression(a, b, ExpressionType.LessThanOrEqual) },
                 { ">=", (a,b)=>CompareToExpression(a, b, ExpressionType.GreaterThanOrEqual) },
                 { ">",  (a,b)=>CompareToExpression(a, b, ExpressionType.GreaterThan) },
                 { "+",  (a,b)=>MathExpression(a,b, ExpressionType.Add) },
                 { "-",  (a,b)=>MathExpression(a,b, ExpressionType.Subtract) },
                 { "*",  (a,b)=>MathExpression(a,b, ExpressionType.Multiply) },
                 { "/",  (a,b)=>MathExpression(a,b, ExpressionType.Divide) },
                 { "%",  (a,b)=>MathExpression(a,b, ExpressionType.Modulo) },
                 { "StartsWith?", (a,b)=> ReservedWordPredicate("StartsWith?", ExpressionHelper.Coerce(a,_string), ExpressionHelper.Coerce(b,_string)) },
                 { "EndsWith?", (a,b)=> ReservedWordPredicate("EndsWith?", ExpressionHelper.Coerce(a,_string), ExpressionHelper.Coerce(b,_string)) },
                 { "Containing?", (a,b)=> ReservedWordPredicate("Containing?", ExpressionHelper.Coerce(a,_string), ExpressionHelper.Coerce(b,_string)) },
                 { "Matching?", (a,b)=> ReservedWordPredicate("Matching?", ExpressionHelper.Coerce(a,_string), ExpressionHelper.Coerce(b,_string)) },
                 { "Equals?", (a,b)=> ReservedWordPredicate("Equals?", ExpressionHelper.Coerce(a,_string), ExpressionHelper.Coerce(b,_string)) }
             };
              
          }
          /// <summary>main entry point</summary>
          public static Expression<Func<TData, bool>> Parse(string s) { return new PredicateParser<TData>(s).Parse(); }
          public static bool TryParse(string s) { try { Parse(s); } catch (Exception e) { Trace.WriteLine("Parsing exception: \n" + e.StackTrace); return false; } return true; }
          private Expression<Func<TData, bool>> Parse() { return Lambda(ParseExpression()); }
          private Expression ParseExpression()   { return ParseOr(); }
          private Expression ParseOr()           { return ParseBinary(ParseAnd, "||"); }
          private Expression ParseAnd()          { return ParseBinary(ParseEquality, "&&"); }
          private Expression ParseEquality()     { return ParseBinary(ParseRelation, "==", "!="); }
          private Expression ParseRelation()     { return ParseBinary(ParseReservedWord, "<", "<=", ">=", ">"); }
          private Expression ParseReservedWord() { return ParseBinary(ParseSum, "StartsWith?", "EndsWith?", "Containing?", "Matching?", "Equals?"); }
          private Expression ParseSum()          { return ParseBinary(ParseMul, "+", "-"); }
          private Expression ParseMul()          { return ParseBinary(ParseUnary, "/", "*", "%"); }          

          private Expression ParseUnary()
          {
            if (CurrOpAndNext("!") != null) return _unOp["!"](ParseUnary());
            if (CurrOpAndNext("-") != null) return _unOp["-"](ParseUnary());
               return ParsePrimary();
          }

          // parsing single or nested identifiers. EBNF: ParseIdent = ident { "." ident } .
          private Expression ParseNestedIdent()
          {
              Expression expr = ParameterMember(CurrOptNext);
              while (CurrOpAndNext(".") != null && IsIdent) expr = Expression.PropertyOrField(expr, CurrOptNext);
              return expr;
          }

          private Expression ParseIndexer()
          {
              var keyValue = Regex.Replace(CurrOptNext, @"^\[(?:\s*)(.*?)(?:\s*)\]$", m => m.Groups[1].Value);

              if (!typeof(IDictionary<string, object>).IsAssignableFrom(typeof(TData)))
                 Abort("unsupported indexer for source type: " + typeof(TData));

              Expression keyExpr = Expression.Constant(keyValue, typeof(string));
              return Expression.Property(_param, "Item", keyExpr);
          }      

          private Expression ParseString()     { return Const(Regex.Replace(CurrOptNext, "^\"(.*)\"$",
                                                 m => m.Groups[1].Value)); }
          private Expression ParseNumber()     { if (IsDouble) return Const(double.Parse(CurrOptNext));
                                                 return Const(int.Parse(CurrOptNext)); }
          private Expression ParsePrimary()
          {
              if (IsBool) return ParseBool();
              if (IsIdent) return ParseNestedIdent();
              if (IsIndexer) return ParseIndexer();
              if (IsString) return ParseString();
              if (IsNumber) return ParseNumber();
              return ParseNested();
          }

          private Expression ParseBool()
          {
              var boolValue = Convert.ToBoolean(CurrOptNext);
              return Expression.Constant(boolValue, typeof(bool));
          }

          private Expression ParseNested()
          {
              if (CurrAndNext != "(") Abort("(...) expected");
              Expression expr = ParseExpression();
              if (CurrOptNext != ")") Abort("')' expected");
              return expr;
          }
          
          /// <summary>generic parsing of binary expressions</summary>
          private Expression ParseBinary(Func<Expression> parse, params string[] ops)
          {
              Expression expr = parse();
              string op;
              while ((op = CurrOpAndNext(ops)) != null) expr = _binaryOperators[op](expr, parse());
              return expr;
          }

          
          #endregion
      }
        


   

  
  }



