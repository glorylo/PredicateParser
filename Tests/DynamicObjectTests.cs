using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DynamicObjectTests
    {
        public dynamic Product { get; set; }


        private void TraceProductAndExpression(string expression)
        {
            var product = Product as IDictionary<string, object>;
            Trace.WriteLine("Expression:  " + expression);
            Trace.WriteLine("Id:  " + product["Id"]);
            Trace.WriteLine("Description:  " + product["Description"]);
            Trace.WriteLine("Price:  " + product["Price"]);
            Trace.WriteLine("Inventory Status:  " + product["Inventory Status"]);
            Trace.WriteLine("Quantity:  " + product["Quantity"]);
            Trace.WriteLine("Best Seller:  " + product["Best Seller"]);
            Trace.WriteLine("Sale Item:  " + product["Sale Item"]);
            Trace.WriteLine("Distance:  " + product["Distance"]);
        }

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
            product["Best Seller"] = true;
            product["Sale Item"] = false;
            product["Distance"] = "0.5 miles";
        }

        [Test]
        public void VerifyTrueProperty()
        {
            var expr = @"[Best Seller]";
            var product = Product as IDictionary<string, object>;
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }
        
        [Test]
        public void VerifyTrue()
        {
            var expr = @"[Best Seller] == true";
            var product = Product as IDictionary<string, object>;
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyFalse()
        {
            var expr = @"[Sale Item] == false";
            var product = Product as IDictionary<string, object>;
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyBoolNotEquals()
        {
            var expr = @"[Sale Item] != true";
            var product = Product as IDictionary<string, object>;
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyNegateBool()
        {
            var expr = @"![Sale Item] == true";
            var product = Product as IDictionary<string, object>;
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyIndexerOnDynamicEquals()
        {
            var expr = @"[Price] == 34.99";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }
        
        [Test]
        public void VerifyIndexerEquals()
        {
            var expr = @"[Inventory Status] == ""In Stock""";
            var product = Product as IDictionary<string, object>;
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyIndexerNotEquals()
        {
            var expr = @"[Inventory Status] != ""Available""";
            var product = Product as IDictionary<string, object>;
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }
        
        [Test]
        public void VerifyIndexerWithEqualsReservedWord()
        {
            var expr = @"[Inventory Status] Equals? ""In Stock""";
            var product = Product as IDictionary<string, object>;
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, product));
        }

        [Test]
        public void VerifyRhsContainingReservedWord()
        {
            var expr = @"""There is Stuff In Stock"" Containing? [Inventory Status]";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyContainingReservedWord()
        {
            var expr = @"Description Containing? ""best widget""";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        
        [Test]
        public void VerifyNotContainingReservedWord()
        {
            var expr = @"!(Description Containing? ""test"")";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyStartsWithReservedWord()
        {
            var expr = @"Description StartsWith? ""The best""";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNotStartsWithReservedWord()
        {
            var expr = @"!(Description StartsWith? ""The test"")";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyIndexerEndsWithReservedWord()
        {
            var expr = @"Description EndsWith? ""wonders""";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyIntPropertyEquals()
        {
            var expr = @"Price == 34.99";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyIntPropertyNotEquals()
        {
            var expr = @"Price != 35.99";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNegateIntPropertyEquals()
        {
            var expr = @"!(Price == 30.99)";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyLessThanCondition()
        {
            var expr = @"Price < 40";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyLessThanOrEqualCondition()
        {
            var expr = @"Price <= 34.99";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyGreaterThanCondition()
        {
            var expr = @"Price > 33.99";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyGreaterThanOrEqualCondition()
        {
            var expr = @"Price >= 34.99";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyOrCondition()
        {
            var expr = @"Price == 44.99 || Quantity > 20";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyAndCondition()
        {
            var expr = @"Price < 50.00 && Quantity >= 30";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNegativeIntCondition()
        {
            var expr = @"-Quantity  == -30";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }
       
        [Test]
        public void VerifyAddCondition()
        {
            var expr = @"Quantity + 10 == 40";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifySubtractCondition()
        {
            var expr = @"Quantity - 10 == 20";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyMultiplyCondition()
        {
            var expr = @"Quantity * 5 == 150";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyDivCondition()
        {
            var expr = @"Quantity / 2 == 15";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyModCondition()
        {
            var expr = @"Quantity % 5 == 0";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNestedMathCondition()
        {
            var expr = @"Quantity + (3 * 5) == 45";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNegativeRealCondition()
        {
            var expr = @"-Price  == -34.99";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyRealNumberAddCondition()
        {
            var expr = @"Price + 10.01 == 45.0";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyRealNumberSubtractCondition()
        {
            var expr = @"Price - 10.99 == 24.0";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyRealMultCondition()
        {
            var expr = @"Price * 2.0 == 69.98";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyRealDivCondition()
        {
            var expr = @"Price / 2.0 == 17.495";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        [Test]
        public void VerifyNumbersInStringsCondition()
        {
            var expr = @"Distance EndsWith? ""miles""";
            TraceProductAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }

        //  doesn't work
/*
        [Test]
        public void VerifyRealModCondition()
        {
            var expr = @"Price % 1.0 == 34.99";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Product));
        }
*/


    }
}
