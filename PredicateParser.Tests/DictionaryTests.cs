using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Diagnostics;

namespace PredicateParser.Tests
{
    [TestFixture]
    class DictionaryTests
    {
        public IDictionary<string, object> Widget { get; set; }

        private void TraceWigetAndExpression(string expression)
        {
            Trace.WriteLine("Expression:  " + expression);
            Trace.WriteLine("Id:  " +  Widget["Id"]);
            Trace.WriteLine("Description:  " + Widget["Description"]);
            Trace.WriteLine("Price:  " + Widget["Price"]);
            Trace.WriteLine("Inventory Status:  " + Widget["Inventory Status"]);
            Trace.WriteLine("Quantity:  " + Widget["Quantity"]);
            Trace.WriteLine("Best Seller:  " + Widget["Best Seller"]);
            Trace.WriteLine("Sale Item:  " + Widget["Sale Item"]);
        }

        [SetUp]
        public void BeforeTest() 
        {
            Widget = Product.Widget;
        }

        [Test]
        public void VerifyTrue()
        {
            var expr = @"[Best Seller] == true";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyFalse()
        {
            var expr = @"[Sale Item] == false";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyBoolNotEquals()
        {
            var expr = @"[Sale Item] != true";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyNegateBool()
        {
            var expr = @"![Sale Item] == true";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyIndexerOnDynamicEquals()
        {
            var expr = @"[Price] == 34.99";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyIndexerEquals()
        {
            var expr = @"[Inventory Status] == ""In Stock""";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyIndexerNotEquals()
        {
            var expr = @"[Inventory Status] != ""Available""";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyIndexerWithEqualsReservedWord()
        {
            var expr = @"[Inventory Status] Equals? ""In Stock""";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyRhsContainingReservedWord()
        {
            var expr = @"""There is Stuff In Stock"" Containing? [Inventory Status]";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyContainingReservedWord()
        {
            var expr = @"[Description] Containing? ""best widget""";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }


        [Test]
        public void VerifyNotContainingReservedWord()
        {
            var expr = @"!([Description] Containing? ""test"")";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyStartsWithReservedWord()
        {
            var expr = @"[Description] StartsWith? ""The best""";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyNotStartsWithReservedWord()
        {
            var expr = @"!([Description] StartsWith? ""The test"")";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyIndexerEndsWithReservedWord()
        {
            var expr = @"[Description] EndsWith? ""wonders""";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyIntPropertyEquals()
        {
            var expr = @"[Price] == 34.99";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyIntPropertyNotEquals()
        {
            var expr = @"[Price] != 35.99";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyNegateIntPropertyEquals()
        {
            var expr = @"!([Price] == 30.99)";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyLessThanCondition()
        {
            var expr = @"[Price] < 40";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyLessThanOrEqualCondition()
        {
            var expr = @"[Price] <= 34.99";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyGreaterThanCondition()
        {
            var expr = @"[Price] > 33.99";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyGreaterThanOrEqualCondition()
        {
            var expr = @"[Price] >= 34.99";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyOrCondition()
        {
            var expr = @"[Price] == 44.99 || [Quantity] > 20";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyAndCondition()
        {
            var expr = @"[Price] < 50.00 && [Quantity] >= 30";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyNegativeIntCondition()
        {
            var expr = @"-[Quantity] == -30";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyAddCondition()
        {
            var expr = @"[Quantity] + 10 == 40";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifySubtractCondition()
        {
            var expr = @"[Quantity] - 10 == 20";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyMultiplyCondition()
        {
            var expr = @"[Quantity] * 5 == 150";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyDivCondition()
        {
            var expr = @"[Quantity] / 2 == 15";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyModCondition()
        {
            var expr = @"[Quantity] % 5 == 0";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyNestedMathCondition()
        {
            var expr = @"[Quantity] + (3 * 5) == 45";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyNegativeRealCondition()
        {
            var expr = @"-[Price]  == -34.99";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyRealNumberAddCondition()
        {
            var expr = @"[Price] + 10.01 == 45.0";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyRealNumberSubtractCondition()
        {
            var expr = @"[Price] - 10.99 == 24.0";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyRealMultCondition()
        {
            var expr = @"[Price] * 2.0 == 69.98";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }

        [Test]
        public void VerifyRealDivCondition()
        {
            var expr = @"[Price] / 2.0 == 17.495";
            TraceWigetAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Widget));
        }
    }
}
