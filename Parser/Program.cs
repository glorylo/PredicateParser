using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Parser
{
    class Profile
    {
        public string Username { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string OnlineStatus { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            var profile = new ExpandoObject() as IDictionary<string, object>;
            profile["Username"] = "Johny";
            profile["Age"] = 33;
            profile["Location"] = "Vancouver";
            profile["Online Status"] = "Online Now";



            var profile2 = new Profile();
            profile2.Age = 32;
            profile2.Location = "Burnaby";

            
            var s = @"[Location == ""Vancouver""";
            try
            {
                 var pred = SimpleExpression.PredicateParser<IDictionary<string, object>>.Parse(s);
                 //var pred = SimpleExpression.PredicateParser<Profile>.Parse(s);
                 Console.WriteLine("String: {0}", s);
                 Console.WriteLine("Expr Tree:  {0}", pred.ToString());
                 var predicateCompiled = pred.Compile();
                 var matchLocation = predicateCompiled(profile);
                 //var matchLocation = predicateCompiled(profile2);
                 Console.WriteLine("Output of prediate: {0}", matchLocation);

            }
            catch (Exception e)
            {
                Console.WriteLine("Parse Error: " + e.Message);

            }

            Console.ReadLine();


        }
    }
}
