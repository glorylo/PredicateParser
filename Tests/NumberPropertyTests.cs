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
            return ExpressionEvaluator.Evaluate(expression, People.John);
        }

        [SetUp]
        public void BeforeTest()
        {
            Subject = People.John;
        }

        [Test]
        public void VerifyIntEqualsProperty()
        {
            var expr = @"Age == 60";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNegateEqualsProperty()
        {
            var expr = @"!(Age == 50)";
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
            var expr = @"Age < 61";
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
            var expr = @"Age < 61 || Age > 59 ";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIntOutOfRangeLowerBoundProperty()
        {
            var expr = @"Age > 60 || Age < 62";
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
        public void VerifyRealNotOutOfRangeBetweenProperty()
        {
            var expr = @"!(Salary > 2400 && Salary < 2350)";
            Assert.IsTrue(EvalulateExpression(expr));
        }
        [Test]
        public void VerifyRealOutOfRangeUpperBoundProperty()
        {
            var expr = @"Salary > 2300 || Salary < 2300.51";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyRealOutOfRangeLowerBoundProperty()
        {
            var expr = @"Salary > 2400.50 || Salary < 2300.60";
            Assert.IsTrue(EvalulateExpression(expr));            
        }

        [Test]
        public void VerifyNegativeIntEquals()
        {
            var expr = @"-Age == -60";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNegativeRealNumberEquals()
        {
            var expr = @"-Salary == -2300.50";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyAdditionProperty()
        {
            var expr = @"Salary + 1050 == 3350.50";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifySubtractionProperty()
        {
            var expr = @"Salary - 1000 == 1300.50";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyMultiplyProperty()
        {
            var expr = @"Salary * 3.5  == 8051.75";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyDivProperty()
        {
            var expr = @"Salary / 2  == 1150.25";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyModProperty()
        {
            var expr = @"Salary % 2  == 0.5";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNestedMathOnProperty()
        {
            var expr = @"(Salary + (20  * 3))  / 2 == 1180.25";
            Assert.IsTrue(EvalulateExpression(expr));
        }


    } 


}
