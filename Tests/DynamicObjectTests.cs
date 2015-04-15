using System;
using System.Collections.Generic;
using System.Dynamic;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DynamicObjectTests
    {
        public dynamic Product { get; set; }

        [SetUp]
        public void BeforeTest()
        {
            Product = new ExpandoObject();
            var product = Product as IDictionary<string, object>;
            product["Id"] = "1234567";
            product["Description"] = "The best widget for your household.  Works wonders";
            product["Price"] = 34.99;
            product["Inventory Status"] = "In Stock";
            product["Quantity"] = 30;
        }

        [Test]
        public void VerifyIndexerEquals()
        {
            var expr = @"[Inventory Status] == ""In Stock""";
            var product = Product as IDictionary<string, object>;
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyIndexerNotEquals()
        {
            var expr = @"[Inventory Status] != ""Available""";
            var product = Product as IDictionary<string, object>;
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }
        
        [Test]
        public void VerifyIndexerWithEqualsReservedWord()
        {
            var expr = @"[Inventory Status] Equals? ""In Stock""";
            var product = Product as IDictionary<string, object>;
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyIndexerContainingReservedWord()
        {
            var expr = @"Description Containing? ""best widget""";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyIndexerNotContainingReservedWord()
        {
            var expr = @"!(Description Containing? ""test"")";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyIndexerStartsWithReservedWord()
        {
            var expr = @"Description StartsWith? ""The best""";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyIndexerNotStartsWithReservedWord()
        {
            var expr = @"!(Description StartsWith? ""The test"")";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyIntPropertyEquals()
        {
            var expr = @"Price == 34.99";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyIntPropertyNotEquals()
        {
            var expr = @"Price != 35.99";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNegateIntPropertyEquals()
        {
            var expr = @"!(Price == 30.99)";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyLessThanCondition()
        {
            var expr = @"Price < 40";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyLessThanOrEqualCondition()
        {
            var expr = @"Price <= 34.99";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyGreaterThanCondition()
        {
            var expr = @"Price > 33.99";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyGreaterThanOrEqualCondition()
        {
            var expr = @"Price >= 34.99";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyOrCondition()
        {
            var expr = @"Price == 44.99 || Quantity > 20";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyAndCondition()
        {
            var expr = @"Price < 50.00 && Quantity >= 30";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyAddCondition()
        {
            var expr = @"Quantity + 10 == 40";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifySubtractCondition()
        {
            var expr = @"Quantity - 10 == 20";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyMultiplyCondition()
        {
            var expr = @"Quantity * 5 == 150";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyDivCondition()
        {
            var expr = @"Quantity /2 == 15";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyModCondition()
        {
            var expr = @"Quantity % 5 == 0";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }


    }
}
