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
        public void VerifyWhiteSpaceEqualsIdentifer()
        {
            var expr = @"[Inventory Status] == ""In Stock""";
            var product = Product as IDictionary<string, object>;
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyWhiteSpaceNotEqualsIdentifer()
        {
            var expr = @"[Inventory Status] != ""Available""";
            var product = Product as IDictionary<string, object>;
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }
        
        [Test]
        public void VerifyWithEqualsReservedWord()
        {
            var expr = @"[Inventory Status] Equals? ""In Stock""";
            var product = Product as IDictionary<string, object>;
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyContainingReservedWord()
        {
            var expr = @"Description Containing? ""best widget""";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNotContainingReservedWord()
        {
            var expr = @"!(Description Containing? ""test"")";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyStartsWithReservedWord()
        {
            var expr = @"Description StartsWith? ""The best""";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNotStartsWithReservedWord()
        {
            var expr = @"!(Description StartsWith? ""The test"")";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyGetProperty()
        {
            var expr = @"Price == 34.99";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyLessThanCondition()
        {
            var expr = @"Price < 40";
            dynamic p = Product;
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, p));
        }

    }
}
