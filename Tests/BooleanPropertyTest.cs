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
            return ExpressionEvaluator.Evaluate(expression, People.John);
        }

        [SetUp]
        public void BeforeTest()
        {
            Subject = People.John;
        }

        [Test]
        public void VerifyIsFalse()
        {
            var expr = @"HasCar == false";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyIsTrue()
        {
            var expr = @"HasSiblings == true";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNotTrue()
        {
            var expr = @"HasCar != true";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNotFalse()
        {
            var expr = @"HasSiblings != false";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNegateFalse()
        {
            var expr = @"!HasCar";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyPropertyAsTrue()
        {
            var expr = @"HasSiblings";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyNegateTrue()
        {
            var expr = @"!HasSiblings == false";
            Assert.IsTrue(EvalulateExpression(expr));
        }

        [Test]
        public void VerifyOrCondition()
        {
            var expr = @"HasSiblings || HasCar";
            Assert.IsTrue(EvalulateExpression(expr));
        }

    }
}
