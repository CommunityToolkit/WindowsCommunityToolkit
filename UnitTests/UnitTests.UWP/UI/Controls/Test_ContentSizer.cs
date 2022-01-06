// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Automation.Peers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Automation.Peers;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    [TestCategory("Test_ContentSizer")]
    public class Test_ContentSizer
    {
        [UITestMethod]
        public void ShouldConfigureContentSizerAutomationPeer()
        {
            const string automationName = "MyContentSizer";
            const string name = "ContentSizer";

            var contentSizer = new ContentSizer();
            var contentSizerAutomationPeer = FrameworkElementAutomationPeer.CreatePeerForElement(contentSizer) as ContentSizerAutomationPeer;

            Assert.IsNotNull(contentSizerAutomationPeer, "Verify that the AutomationPeer is ContentSizerAutomationPeer.");

            contentSizer.Name = name;
            Assert.IsTrue(contentSizerAutomationPeer.GetName().Contains(name), "Verify that the UIA name contains the given Name of the ContentSizer.");

            contentSizer.SetValue(AutomationProperties.NameProperty, automationName);
            Assert.IsTrue(contentSizerAutomationPeer.GetName().Contains(automationName), "Verify that the UIA name contains the given AutomationProperties.Name of the ContentSizer.");
        }
    }
}
