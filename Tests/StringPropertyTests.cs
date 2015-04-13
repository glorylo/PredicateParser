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

        #region String Property
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

        #endregion


    }
}
