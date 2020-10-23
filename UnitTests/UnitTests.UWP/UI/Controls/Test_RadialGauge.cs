using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.UI.Xaml.Automation.Peers;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public class Test_RadialGauge
    {
        /// <summary>
        /// That the UIA name is valid and makes sense
        /// </summary>
        [TestCategory("Test_TextToolbar_Localization")]
        [UITestMethod]
        public void VerifyUIAName()
        {
            var gauge = new RadialGauge()
            {
                Minimum = 0,
                Maximum = 100,
                Value = 20
            };

            var gaugePeer = FrameworkElementAutomationPeer.CreatePeerForElement(gauge);

            Assert.IsTrue(gaugePeer.GetName().Contains(gauge.Value.ToString()), "Verify that the UIA name contains the value of the RadialGauge.");
            Assert.IsTrue(gaugePeer.GetName().Contains("no unit"), "The UIA name should indicate that unit was not specified.");

            gauge.Unit = "KM/H";
            Assert.IsTrue(gaugePeer.GetName().Contains(gauge.Unit), "The UIA name should report the unit of the RadialGauge.");
        }

    }
}
