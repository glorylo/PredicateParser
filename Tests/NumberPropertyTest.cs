using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class NumberPropertyTest
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
            Subject = new Person { FirstName = "John", LastName = "Smith", Age = 60, Salary = 2300.50, PostalCode = "V5H 0A7" };
        }

        [Test]
        public void VerifyIntEqualsProperty()
        {
            var expr = @"Age == 60";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIntGreaterOrEqualsProperty()
        {
            var expr = @"Age >= 60";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIntLessThanProperty()
        {
            var expr = @"Age < 60";
            Assert.IsFalse(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIntLessThanEqualsProperty()
        {
            var expr = @"Age <= 60";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIntBetweenProperty()
        {
            var expr = @"Age > 50 && Age < 70";
            Assert.IsTrue(EvalulateExpression(expr));
        }


    }
}
