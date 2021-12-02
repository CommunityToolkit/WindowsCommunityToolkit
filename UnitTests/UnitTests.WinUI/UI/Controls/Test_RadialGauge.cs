// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace UnitTests.WinUI.UI.Controls
{
    [TestClass]
    public class Test_RadialGauge
    {
        /// <summary>
        /// Verifies that the UIA name is valid and makes sense
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