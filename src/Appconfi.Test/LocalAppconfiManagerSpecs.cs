using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Appconfi.Test
{
    [TestClass]
    public class LocalAppconfiManagerSpecs
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Given_a_null_json_Should_throw_an_exception()
        {
            new LocalAppconfiManager(null);
        }

        [TestMethod]
        public void Given_a_enabled_feature_Should_return_true()
        {
            var manager = new LocalAppconfiManager(@"{""feature_a"": true}");
            Assert.IsTrue(manager.IsFeatureEnabled("feature_a"));
        }

        [TestMethod]
        public void Given_a_invalid_feature_Should_return_defaultValue()
        {
            var manager = new LocalAppconfiManager(@"{""feature_a"": true}");
            Assert.IsTrue(manager.IsFeatureEnabled("feature_b",true));
        }

        [TestMethod]
        public void Given_a_disabled_feature_Should_return_false()
        {
            var manager = new LocalAppconfiManager(@"{""feature_a"":{""isEnabled"":false,""enabledFor"":{""name"":""percentage"",""parameters"":{""percent"":90}}}}");
            Assert.IsFalse(manager.IsFeatureEnabled("feature_a", true));
        }

        [TestMethod]
        public void Given_an_invlid_feature_Should_return_default_value()
        {
            var manager = new LocalAppconfiManager(@"{""feature_a"":{""enabledFor"":{""name"":""percentage"",""parameters"":{""percent"":90}}}}");
            Assert.IsTrue(manager.IsFeatureEnabled("feature_a", true));

            manager = new LocalAppconfiManager(@"{""feature_a"":{""isEnabled"":10,""enabledFor"":{""name"":""percentage"",""parameters"":{""percent"":90}}}}");
            Assert.IsTrue(manager.IsFeatureEnabled("feature_a", true));
        }

        [TestMethod]
        public void Given_a_rule_Should_return_enabled()
        {
            var manager = new LocalAppconfiManager(@"{""feature_a"":{""enabledFor"":{""name"":""percentage"",""parameters"":{""percent"":100}}}}");
            Assert.IsTrue(manager.IsFeatureEnabled("feature_a",new User("1234"), false));

            manager = new LocalAppconfiManager(@"{""feature_a"":{""enabledFor"":{""name"":""percentage"",""parameters"":{""percent"":0}}}}");
            Assert.IsFalse(manager.IsFeatureEnabled("feature_a", new User("1234"), true));
        }

        [TestMethod]
        public void Given_an_invalid_rule_Should_return_defaultValue()
        {
            var manager = new LocalAppconfiManager(@"{""feature_a"": false}");
            Assert.IsTrue(manager.IsFeatureEnabled("feature_a", new User("1234"), true));
        }


    }
}
