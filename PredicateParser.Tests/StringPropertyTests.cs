using System.Diagnostics;
using NUnit.Framework;
using PredicateParser;


namespace PredicateParser.Tests
{
    [TestFixture]
    public class StringPropertyTests
    {

        public Person Subject { get; set; }

        private bool EvalulateExpression(string expression)
        {
            Subject.Inspect();
            return ExpressionEvaluator.Evaluate(expression, Subject);
        }

        [SetUp]
        public void BeforeTest()
        {
            Subject = People.John;
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
        public void VerifyNegataEqualsProperty()
        {
            var expr = @"!(FirstName == ""Sarah"")";
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

        [Test]
        public void VerifyPropertyMatching()
        {
            var expr = @"FirstName Matching? ""Sarah|John""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyNotMatching()
        {
            var expr = @"FirstName Matching? ""[a-z]\d[a-z]\d[a-z]\d""";
            Assert.IsFalse(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyAndStringPropertyCondition()
        {
            var expr = @"Firstname == ""John"" && Lastname EndsWith? ""mith""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyOrStringPropertyCondition()
        {
            var expr = @"Firstname == ""Joey"" || Lastname == ""Smith""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNestedStringProperty()
        {
            var expr = @"Address.City == ""Vancouver""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNestedStringContainsProperty()
        {
            var expr = @"Address.Street Containing? ""Robson""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNestedStringNotContainingProperty()
        {
            var expr = @"!(Address.Street Containing? ""Hastings"")";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNestedStringNotEqualsProperty()
        {
            var expr = @"Address.City != ""Burnaby""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNestedUnknownProperty()
        {
            var expr = @"Address.Zip  == ""90210""";
            Assert.Throws(typeof(System.ArgumentException), () => EvalulateExpression(expr));
        }
        [Test]
        public void VerifyNestedPropertyMatching()
        {
            var expr = @"Address.PostalCode Matching? ""[A-Z]\d[A-Z]\d[A-Z]\d""";
            Assert.IsTrue(EvalulateExpression(expr));
        }

    }
}
