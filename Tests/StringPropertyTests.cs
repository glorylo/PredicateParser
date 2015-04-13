using System.Diagnostics;
using NUnit.Framework;
using PredicateParser;


namespace Tests
{
    [TestFixture]
    public class StringPropertyTests
    {
        public Person Subject  { get; set; }

        private bool EvalulateExpression(string expression)
        {
            Subject.Inspect();
            return ExpressionEvaluator.Evaluate(expression, Subject);
        }

        [SetUp]
        public void BeforeTest()
        {
            Subject = new Person { FirstName = "John", LastName = "Smith", Age = 60, Salary = 2300.50, PostalCode = "V5H 0A7"};
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
        public void VerifyPropertyStartsWith()
        {
            var expr = @"Firstname StartsWith? ""Jo""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyNotStartsWith()
        {
            var expr = @"Firstname StartsWith? ""Jot""";
            Assert.IsFalse(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyContains()
        {
            var expr = @"Firstname Containing? ""oh""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyNotContains()
        {
            var expr = @"Lastname Containing? ""at""";
            Assert.IsFalse(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyEndsWith()
        {
            var expr = @"Lastname EndsWith? ""mith""";
            Assert.IsTrue(EvalulateExpression(expr));
        }


        [Test]
        public void VerifyPropertyNotEndsWith()
        {
            var expr = @"Lastname EndsWith? ""math""";
            Assert.IsFalse(EvalulateExpression(expr));
        }


    }
}
