using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Appconfi.Test
{
    [TestClass]
    public class RuleSpecs
    {
        [TestMethod]
        public void Given_a_0_percent_rule_Should_not_be_a_target()
        {
            var rule = new RulePercentage { Percent = 0 };
            var isTarget =  rule.IsTarget(new User("1234"));

            Assert.IsFalse(isTarget);
        }

        [TestMethod]
        public void Given_a_100_percent_rule_Should_be_a_target()
        {
            var rule = new RulePercentage { Percent = 100 };
            var isTarget = rule.IsTarget(new User("1234"));

            Assert.IsTrue(isTarget);
        }

        [TestMethod]
        public void Given_a_50_percent_rule_Should_be_a_target()
        {
            var rule = new RulePercentage { Percent = 50 };
            var isTarget = rule.IsTarget(new User("1234"));

            Assert.IsTrue(isTarget);
        }

        [TestMethod]
        public void Given_a_40_percent_rule_Should_not_be_a_target()
        {
            var rule = new RulePercentage { Percent = 40 };
            var isTarget = rule.IsTarget(new User("1234"));

            Assert.IsFalse(isTarget);
        }


        [TestMethod]
        public void Given_a_property_rule_with_an_invalid_option_Should_not_be_a_target()
        {
            var rule = new ByPropertyRule { Option = "invalid"};
            var isTarget = rule.IsTarget(new User("1234"));

            Assert.IsFalse(isTarget);
        }

        [TestMethod]
        [DataRow("isin", "id", "1234", true)]
        [DataRow("isin", "id", "4321", false)]
        [DataRow("isnotin", "id", "4321", true)]
        [DataRow("isnotin", "id", "1234", false)]
        [DataRow("contains", "id", "1234", true)]
        [DataRow("contains", "id", "12", true)]
        [DataRow("contains", "id", "5", false)]
        [DataRow("contains", "id", "", false)]
        [DataRow("contains", "id", null, false)]
        [DataRow("notcontains", "id", "5", true)]
        [DataRow("notcontains", "id", "1", false)]
        public void PropertyRuleTestCases(string option, string property, string values, bool expected)
        {
            var rule = new ByPropertyRule { Option = option, Property = property, Values = values };
            var isTarget = rule.IsTarget(new User("1234") { { "country", "ES"}});

            Assert.AreEqual(expected, isTarget);
        }
    }
}
