using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appconfi.Test
{
    [TestClass]
    public class AppconfiManagerSpec
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AppconfiManager_GetSetting_NullKey_ThrowAnException()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var manager = new AppconfiManager(configurationStoreMock.Object);

            var value = manager.GetSetting(null);
            Assert.IsNull(value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AppconfiManager_IsFeatureEnabled_NullKey_ThrowAnException()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var manager = new AppconfiManager(configurationStoreMock.Object);

            var value = manager.IsFeatureEnabled(null);
            Assert.IsNull(value);
        }


        [TestMethod]
        public void AppconfiManager_GetSetting_ValidKey_ReturnSetting()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var configuration = new ApplicationConfiguration
            {
                Settings = new Dictionary<string, string> { { "setting", "value" } }
            };

            //Given a version
            configurationStoreMock.Setup(x => x.GetVersionAsync()).Returns(Task.FromResult("1"));
            
            //And given a valid configuration
            configurationStoreMock.Setup(x => x.GetConfigurationAsync()).Returns(Task.FromResult(configuration));

            var manager = new AppconfiManager(configurationStoreMock.Object);
            var value = manager.GetSetting("setting");

            Assert.AreEqual("value", value);

        }

        [TestMethod]
        public void AppconfiManager_IsFeatureEnabled_ValidKey_ReturnToggle()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var configuration = new ApplicationConfiguration
            {
                Toggles = new Dictionary<string, string> { { "feature.toggle", "on" } }
            };

            //Given a version
            configurationStoreMock.Setup(x => x.GetVersionAsync()).Returns(Task.FromResult("1"));

            //And given a valid configuration
            configurationStoreMock.Setup(x => x.GetConfigurationAsync()).Returns(Task.FromResult(configuration));

            var manager = new AppconfiManager(configurationStoreMock.Object);
            var isEnabled = manager.IsFeatureEnabled("feature.toggle");

            Assert.IsTrue(isEnabled);

        }

        [TestMethod]
        public void AppconfiManager_IsFeatureEnabled_ValidKey_ReturnFromCache()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var configuration = new ApplicationConfiguration();
            
            //Given a version
            configurationStoreMock.Setup(x => x.GetVersionAsync()).Returns(Task.FromResult("1"));
            //And given a cache
            Func<string,bool> cache = (key) => key == "feature.toggle";

            var manager = new AppconfiManager(configurationStoreMock.Object, TimeSpan.FromSeconds(1), null, cache);
            var isEnabled = manager.IsFeatureEnabled("feature.toggle");

            Assert.IsTrue(isEnabled);
        }

        [TestMethod]
        public void AppconfiManager_GetSetting_ValidKey_ReturnFromCache()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var configuration = new ApplicationConfiguration();

            //Given a version
            configurationStoreMock.Setup(x => x.GetVersionAsync()).Returns(Task.FromResult("1"));
            //And given a cache
            Func<string, string> cache = (key) => key == "cache-setting"? "value": "invalid";

            var manager = new AppconfiManager(configurationStoreMock.Object, TimeSpan.FromSeconds(1), cache, null);
            var value = manager.GetSetting("cache-setting");

            Assert.AreEqual("value",value);
        }
    }
}
