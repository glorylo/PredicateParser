using System.Diagnostics;
using NUnit.Framework;
using PredicateParser;


namespace Tests
{

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
    }

    [TestFixture]
    public class PropertyFieldTests
    {
        public Person Subject  { get; set; }

        private bool EvalulateExpression(string expression)
        {
            var predicate = PredicateParser<Person>.Parse(expression);
            var compiledPredicate = predicate.Compile();
            Trace.WriteLine(compiledPredicate);
            return compiledPredicate(Subject);
        }

        [SetUp]
        public void BeforeTest()
        {
            Subject = new Person { FirstName = "John", LastName = "Smith", Age = 60, Salary = 2300.50 };
        }

        [Test]
        public void VerifyParseValidProperty()
        {
            var expr = @"FirstName == ""John""";
            var parsedCorrectly = PredicateParser<Person>.TryParse(expr);
            Assert.IsTrue(parsedCorrectly);
        }

        [Test]
        public void VerifyPropertyEquals()
        {
            var expr = @"FirstName == ""John""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyNotEquals()
        {
            var expr = @"FirstName != ""Sarah""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyEqualsReservedWord()
        {
            var expr = @"FirstName Equals? ""John""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyNotEqualsReservedWord()
        {
            var expr = @"FirstName Equals? ""Sarah""";
            Assert.IsFalse(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIntEqualsProperty()
        {
            var expr = @"Age == 60";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyGreaterOrEqualsProperty()
        {
            var expr = @"Age >= 60";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyLessThanProperty()
        {
            var expr = @"Age < 60";
            Assert.IsFalse(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyLessThanEqualsProperty()
        {
            var expr = @"Age <= 60";
            Assert.IsTrue(EvalulateExpression(expr));
        }    

    }
}
