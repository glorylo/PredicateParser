using NUnit.Framework;
using PredicateParser.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredicateParser.Tests
{
    [TestFixture]
    public class EnumTests
    {
        public dynamic Person;

        [SetUp]
        public void BeforeTest()
        {
            Person = People.John;
        }

        [Test]
        public void TestRightEnum()
        {
            var expr = "Gender Equals? \"MALE\"";
            var gender = Person.Gender.ToString();
            Assert.IsTrue(ExpressionEvaluator.Evaluate(expr, Person));
        }

        [Test]
        public void TestWrongEnum()
        {
            var expr = "Gender Equals? \"FEMALE\"";
            Assert.IsFalse(ExpressionEvaluator.Evaluate(expr, Person));
        }
    }
}
