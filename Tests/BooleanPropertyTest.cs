using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class BooleanPropertyTest
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
            Subject = new Person { FirstName = "John", LastName = "Smith", Age = 60, Salary = 2300.50, PostalCode = "V5H0A7", HasCar = false, HasSiblings = true};
        }

        [Test]
        public void VerifyIsFalse()
        {
            var expr = @"HasCar == false";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr,Subject));
        }

        [Test]
        public void VerifyIsTrue()
        {
            var expr = @"HasSiblings == true";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Subject));
        }

        [Test]
        public void VerifyNotTrue()
        {
            var expr = @"HasCar != true";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Subject));
        }

        [Test]
        public void VerifyNotFalse()
        {
            var expr = @"HasSiblings != false";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Subject));
        }

        [Test]
        public void VerifyNegateFalse()
        {
            var expr = @"!HasCar";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Subject));
        }

        [Test]
        public void VerifyPropertyAsTrue()
        {
            var expr = @"HasSiblings";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Subject));
        }

        [Test]
        public void VerifyNegateTrue()
        {
            var expr = @"!HasSiblings == false";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Subject));
        }

        [Test]
        public void VerifyOrCondition()
        {
            var expr = @"HasSiblings || HasCar";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Subject));
        }

    }
}
