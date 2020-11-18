using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Appconfi.Test
{
    [TestClass]
    public class AppconfiManagerSpecs
    {
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
            var configuration = new ApplicationConfiguration { { "feature.toggle", true } };

            //Given a version
            configurationStoreMock.Setup(x => x.GetVersion()).Returns("1");

            //And given a valid configuration
            configurationStoreMock.Setup(x => x.GetFeatures()).Returns(configuration);

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
    }
}
