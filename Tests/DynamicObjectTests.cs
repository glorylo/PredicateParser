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
            Product.Id = "1234567";
            Product.Description = "The best widget for your household.  Works wonders";
            Product.Price = "34.99";
            ((IDictionary<string, object>) Product)["Inventory Status"] = "In Stock";
            Product.Quantity = 30;
        }

        [Test]
        public void VerifyWhiteSpaceIdentifer()
        {
            var profile = new ExpandoObject() as IDictionary<string, object>;
            profile["Username"] = "Johny";
            profile["Age"] = 33;
            profile["Location"] = "Vancouver";
            profile["Online Status"] = "Online Now";


            var expr = @"[Online Status] Equals? ""Online Now""";
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, profile));

        }

    }
}
