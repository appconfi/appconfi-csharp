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
            configurationStoreMock.Setup(x => x.GetVersion()).Returns("1");

            //And given a valid configuration
            configurationStoreMock.Setup(x => x.GetConfiguration()).Returns(configuration);

            var manager = new AppconfiManager(configurationStoreMock.Object);
            var value = manager.GetSetting("setting");

            Assert.AreEqual("value", value);

        }

        [TestMethod]
        public void AppconfiManager_ForceRefresh_GetSetting_AskForNewVersion()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var configuration = new ApplicationConfiguration
            {
                Settings = new Dictionary<string, string> { { "setting", "value" } }
            };

            var manager = new AppconfiManager(configurationStoreMock.Object);

            manager.ForceRefresh();

            configurationStoreMock.Verify(x => x.GetVersion(), Times.Once);

        }


        [TestMethod]
        public void AppconfiManager_StartMonitoring_IsMonitoring_ReturnTrue()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var manager = new AppconfiManager(configurationStoreMock.Object);

            manager.StartMonitor();

            Assert.IsTrue(manager.IsMonitoring);
        }

        [TestMethod]
        public void AppconfiManager_StopMonitoring_IsMonitoring_ReturnFalse()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var manager = new AppconfiManager(configurationStoreMock.Object);

            manager.StartMonitor();
            manager.StopMonitor();

            Assert.IsFalse(manager.IsMonitoring);
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
            configurationStoreMock.Setup(x => x.GetVersion()).Returns("1");

            //And given a valid configuration
            configurationStoreMock.Setup(x => x.GetConfiguration()).Returns(configuration);

            var manager = new AppconfiManager(configurationStoreMock.Object);
            var isEnabled = manager.IsFeatureEnabled("feature.toggle");

            Assert.IsTrue(isEnabled);

        }

        [TestMethod]
        public void AppconfiManager_IsFeatureEnabled_ValidKey_ReturnFromDefault()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var configuration = new ApplicationConfiguration();

            //Given a version
            configurationStoreMock.Setup(x => x.GetVersion()).Returns("1");

            var manager = new AppconfiManager(configurationStoreMock.Object, TimeSpan.FromSeconds(1));
            var isEnabled = manager.IsFeatureEnabled("feature.toggle", true);

            Assert.IsTrue(isEnabled);
        }

        [TestMethod]
        public void AppconfiManager_GetSetting_ValidKey_ReturnFromDefault()
        {
            var configurationStoreMock = new Mock<IConfigurationStore>();
            var configuration = new ApplicationConfiguration();

            //Given a version
            configurationStoreMock.Setup(x => x.GetVersion()).Returns("1");

            var manager = new AppconfiManager(configurationStoreMock.Object, TimeSpan.FromSeconds(1));
            var value = manager.GetSetting("cache-setting", "value");

            Assert.AreEqual("value", value);
        }
    }
}
