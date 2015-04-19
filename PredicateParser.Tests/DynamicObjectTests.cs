using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using NUnit.Framework;

namespace PredicateParser.Tests
{
    [TestFixture]
    public class DynamicObjectTests
    {
        public dynamic Car { get; set; }

        #region trace
        private void TraceCarAndExpression(string expression)
        {
            Trace.WriteLine("Exression: " + expression);
            Product.InspectCar();
        }
        #endregion

        [SetUp]
        public void BeforeTest()
        {
            Car = Product.CreateCar();
        }

        [Test]
        public void VerifyEqualsMake()
        {
            var expr = @"Make == ""Honda""";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyNotEqualsMake()
        {
            var expr = @"Make != ""Toyota""";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyNegateEqualsModel()
        {
            var expr = @"!(Model == ""Taurus"")";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyDoorsGreaterThan()
        {
            var expr = @"NumberOfDoors > 2";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyDoorsGreaterThanEquals()
        {
            var expr = @"NumberOfDoors >= 4";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyYearLessThan()
        {
            var expr = @"Year < 2003";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyYearLessThanEquals()
        {
            var expr = @"Year <= 2002";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyPriceAddAirConditioning()
        {
            var expr = @"Price + 1200.99 ==  16200.99";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyPriceMultWithTaxRate()
        {
            var expr = @"Price * 1.05 == 15750.0";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyPriceDivPayment()
        {
            var expr = @"Price / 10.0 ==  1500.00";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyFourDoorsOrHybrid()
        {
            var expr = @"Hybrid || NumberOfDoors >= 4";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyNotHybridAndHasMp3()
        {
            var expr = @"!Hybrid && HasMp3";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyDealerContainsImport()
        {
            var expr = @"Dealer.Name Containing? ""Import""";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyDealerStartsWith604()
        {
            var expr = @"Dealer.PhoneNumber StartsWith? ""604""";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyDealerEndsWith()
        {
            var expr = @"Dealer.Distance EndsWith? ""kilometers""";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyDealerSalesHigherThan()
        {
            var expr = @"Dealer.HighestSale > 51000.00";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyTopSellerOrRank()
        {
            var expr = @"(Dealer.TopSeller || Dealer.Rank > 12) && Price < 16000.00";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }

        [Test]
        public void VerifyModPriceBy1()
        {
            var expr = @"Price % 5.0 == 0";
            TraceCarAndExpression(expr);
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Car));
        }


    }
}
