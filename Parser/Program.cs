using System;
using System.Collections.Generic;
using System.Dynamic;

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
            profile["Online Status"] = "Last Online";

//
//            var profile2 = new Profile();
//            profile2.Age = 32;
//            profile2.Location = "Burnaby";

            //var s = @"Location Matching? ""couver$"")";
            var s = @"10 / 2 >= 3 + 3";
            try
            {
                 var pred = SimpleExpression.PredicateParser<IDictionary<string, object>>.Parse(s);
                 //var pred = SimpleExpression.PredicateParser<Profile>.Parse(s);
                 Console.WriteLine("String: {0}", s);
                 Console.WriteLine("Expr Tree:  {0}", pred);
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
