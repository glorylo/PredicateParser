using System;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class NumberPropertyTests
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
            Subject = new Person { FirstName = "John", LastName = "Smith", Age = 60, Salary = 2300.50, PostalCode = "V5H0A7" };
        }

        [Test]
        public void VerifyIntEqualsProperty()
        {
            var expr = @"Age == 60";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIntNotEqualsProperty()
        {
            var expr = @"Age != 50";
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
            Subject.Age = 50;
            Assert.IsTrue(EvalulateExpression(expr));
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

        [Test]
        public void VerifyIntOutOfRangeUpperBoundProperty()
        {
            var expr = @"Age > 60 || Age < 60";
            Subject.Age = 61;
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIntOutOfRangeLowerBoundProperty()
        {
            var expr = @"Age > 60 || Age < 60";
            Subject.Age = 59;
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealNumberEquals()
        {
            var expr = @"Salary == 2300.50";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealNumberNotEquals()
        {
            var expr = @"Salary != 2200.50";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealNumberGreaterThan()
        {
            var expr = @"Salary > 2200";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealNumberGreaterThanEquals()
        {
            var expr = @"Salary >= 1200";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealNumberLessThan()
        {
            var expr = @"Salary < 2300.51";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealNumberLessThanEquals()
        {
            var expr = @"Salary <= 2300.50";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealBetweenProperty()
        {
            var expr = @"Salary > 2000 && Salary < 3000";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealOutOfRangeUpperBoundProperty()
        {
            var expr = @"Salary > 2300.60 || Salary < 2000";
            Subject.Salary = 2300.61;
            Assert.IsTrue(EvalulateExpression(expr));
        }

        public void VerifyRealOutOfRangeLowerBoundProperty()
        {
            var expr = @"Salary > 2300.60 || Salary < 2000";
            Subject.Salary = 1999.99;
            Assert.IsTrue(EvalulateExpression(expr));
            
        }

    }


}
