using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using PredicateParser.Extensions;

// Forked version of PredicateParser originally by Andreas Gieriet
// See this Article:  http://www.codeproject.com/Articles/355513/Invent-your-own-Dynamic-LINQ-parser
namespace PredicateParser
  {
      public abstract class PredicateParser
      {
          #region built-in reserved words

          protected static readonly string[] ReservedWords = {"StartsWith?", "EndsWith?", "Containing?", "Matching?", "Equals?"};

          protected static readonly IDictionary<string, Func<Expression, Expression, Expression>> BuiltInReservedWords = 
              new Dictionary<string, Func<Expression, Expression, Expression>>
              {
              { "StartsWith?", StringExpression.StartsWith },
              { "EndsWith?", StringExpression.EndsWith },
              { "Containing?", StringExpression.Containing },
              { "Matching?", StringExpression.Matching },
              { "Equals?", StringExpression.Equals },
          }; 
          #endregion
          #region scanner

          protected static readonly string[] Operators = { "||", "&&", "==", "!=", "<=", ">=", "+", "-", "/", "*"};
          protected static readonly string[] Booleans = { "true", "false" };
          protected static readonly string Null = "null";

          private static readonly string _pattern = @"\s*(" + string.Join("|", new[]
          {              
              string.Join("|", ReservedWords.Select(Regex.Escape)), // reserved words  
              @"""((?:\\.|[^""])*)""", // not sure if this made a diff.  
              // operators and punctuation that are longer than one char: longest first
              string.Join("|", Operators.Select(Regex.Escape)),  // operators
              @"\d+(?:\.\d+)?", // number with optional decimal part
              @"\w+", // word
              @"\[(?:\s*)((?:\w+\s*)+)(?:\s*)\]", // white space words in square brackets
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
          protected bool IsReservedWord { get { return ReservedWords.Contains(Curr);  } }
          protected bool IsString { get { return Ch == '"'; } }
          protected bool IsIndexer { get { return Ch == '['; }}
          protected bool IsIdent 
          { 
              get 
              {
                  char c = Ch;
                  return !IsBool && !IsReservedWord && 
                      (char.IsLower(c) || char.IsUpper(c) || c == '_'); 
              } 
          }

          /// <summary>throw an argument exception</summary>
          protected static void Abort(string msg) { throw new ArgumentException("Parse Error: " + (msg ?? "unknown error")); }
          /// <summary>get the current item of the stream or an empty string after the end</summary>
          protected string Curr { get { return _tokens.Current ?? string.Empty; }}
          /// <summary>get current and move to the next token (error if at end of stream)</summary>
          protected string CurrAndNext { get { string s = Curr; if (!Move()) Abort("data expected"); return s; } }
          /// <summary>get current and move to the next token if available</summary>
          protected string CurrOptNext { get { string s = Curr; Move(); return s; } }
          /// <summary>moves forward if current token matches and returns that (next token must exist)</summary>
          protected string CurrOpAndNext(params string[] operators)
          {
              string s = operators.Contains(Curr) ? Curr : null;
              if (s != null && !Move()) Abort("data expected");
              return s;
          }
          #endregion
      }

      public class PredicateParser<TData>: PredicateParser
      {
          #region code generator          

          private Dictionary<string, Func<Expression, Expression, Expression>> _binaryOperators; 

          private Dictionary<string, Func<Expression, Expression>> _unaryOperators =
              new Dictionary<string, Func<Expression, Expression>>()
          {
              { "!", BooleanExpression.Not },
              { "-", MathExpression.Negate },
          };

          /// <summary>create a constant of a value</summary>
          private static ConstantExpression Const(object v) { return Expression.Constant(v); }

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
                 { "||", BooleanExpression.Or },
                 { "&&", BooleanExpression.And },
                 { "==", (lhs,rhs)=>CompareExpression.CompareTo(lhs, rhs, ExpressionType.Equal) },
                 { "!=", (lhs,rhs)=>CompareExpression.CompareTo(lhs, rhs, ExpressionType.NotEqual) },
                 { "<",  (lhs,rhs)=>CompareExpression.CompareTo(lhs, rhs, ExpressionType.LessThan) },
                 { "<=", (lhs,rhs)=>CompareExpression.CompareTo(lhs, rhs, ExpressionType.LessThanOrEqual) },
                 { ">=", (lhs,rhs)=>CompareExpression.CompareTo(lhs, rhs, ExpressionType.GreaterThanOrEqual) },
                 { ">",  (lhs,rhs)=>CompareExpression.CompareTo(lhs, rhs, ExpressionType.GreaterThan) },
                 { "+",  (lhs,rhs)=>MathExpression.MathOp(lhs,rhs, ExpressionType.Add) },
                 { "-",  (lhs,rhs)=>MathExpression.MathOp(lhs,rhs, ExpressionType.Subtract) },
                 { "*",  (lhs,rhs)=>MathExpression.MathOp(lhs,rhs, ExpressionType.Multiply) },
                 { "/",  (lhs,rhs)=>MathExpression.MathOp(lhs,rhs, ExpressionType.Divide) },
                 { "%",  (lhs,rhs)=>MathExpression.MathOp(lhs,rhs, ExpressionType.Modulo) },
                 { "StartsWith?", (lhs,rhs)=> BuiltInReservedWords["StartsWith?"](lhs, rhs) },
                 { "EndsWith?", (lhs,rhs)=> BuiltInReservedWords["EndsWith?"](lhs, rhs) },
                 { "Containing?", (lhs,rhs)=> BuiltInReservedWords["Containing?"](lhs, rhs) },
                 { "Matching?", (lhs,rhs)=> BuiltInReservedWords["Matching?"](lhs, rhs) },
                 { "Equals?", (lhs,rhs)=> BuiltInReservedWords["Equals?"](lhs, rhs) }
             };              
          }

          public static bool TryParse(string s) 
          { 
              try { Parse(s); } 
              catch (Exception e) 
              { 
                 Trace.WriteLine("Parsing exception: \n" + e.StackTrace); 
                 return false; 
              } 
              return true; 
          }
          
          /// <summary>main entry point</summary>
          public static Expression<Func<TData, bool>> Parse(string s) { return new PredicateParser<TData>(s).Parse(); }
          private Expression<Func<TData, bool>> Parse() { return Lambda(ExpressionHelper.Coerce(ParseExpression(), typeof(bool))); }
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
            if (CurrOpAndNext("!") != null) return _unaryOperators["!"](ParseUnary());
            if (CurrOpAndNext("-") != null) return _unaryOperators["-"](ParseUnary());
               return ParsePrimary();
          }

          // parsing single or nested identifiers. EBNF: ParseIdent = ident { "." ident } .
          private Expression ParseNestedIdent()
          {
              Expression expr = ParameterMemberExpression.Member(_param, CurrOptNext);
              while (CurrOpAndNext(".") != null && IsIdent) 
                  expr = ParameterMemberExpression.Member(expr, CurrOptNext);
              return expr;
          }

          private Expression ParseIndexer()
          {
              var key = Regex.Replace(CurrOptNext, @"^\[(?:\s*)(.*?)(?:\s*)\]$", m => m.Groups[1].Value);

              if (!typeof (IDictionary<string, object>).IsAssignableFrom(typeof (TData)))
                  Abort("unexpected source object type of:  " + typeof(TData));
              
              return ParameterMemberExpression.GetDictionaryValue(_param, key);
          }      

          private Expression ParseString()     { return Const(Regex.Replace(CurrOptNext, "^\"(.*)\"$",
                                                 m => m.Groups[1].Value)); }
          private Expression ParseNumber()     { if (IsDouble) return Const(double.Parse(CurrOptNext));
                                                 return Const(int.Parse(CurrOptNext)); }
          private Expression ParsePrimary()
          {
              if (IsBool) return ParseBool();
              if (IsString) return ParseString();
              if (IsNumber) return ParseNumber();
              if (IsIndexer) return ParseIndexer();
              if (IsIdent) return ParseNestedIdent();
              return ParseNested();
          }

          private Expression ParseBool()
          {
              var boolValue = Convert.ToBoolean(CurrOptNext);
              return Expression.Constant(boolValue, typeof(bool));
          }

          private Expression ParseNested()
          {
              if (CurrAndNext != "(") 
                  Abort("(...) expected");
              Expression expr = ParseExpression();
              if (CurrOptNext != ")") 
                  Abort("')' expected");
              return expr;
          }
          
          /// <summary>generic parsing of binary expressions</summary>
          private Expression ParseBinary(Func<Expression> parse, params string[] operators)
          {
              Expression expr = parse();
              string op;
              while ((op = CurrOpAndNext(operators)) != null) 
                  expr = _binaryOperators[op](expr, parse());
              return expr;
          }
          #endregion
      }
  
  }



