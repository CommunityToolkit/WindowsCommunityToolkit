// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml.Automation;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.UI.Xaml.Automation.Peers;
using Microsoft.Toolkit.Uwp.UI.Automation.Peers;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    [TestCategory("Test_Carousel")]
    public class Test_Carousel
    {
        [UITestMethod]
        public void ShouldConfigureCarouselAutomationPeer()
        {
            const string automationName = "MyAutomationPhotoItems";
            const string name = "MyPhotoItems";

            var items = new ObservableCollection<PhotoDataItem> { new PhotoDataItem { Title = "Hello" }, new PhotoDataItem { Title = "World" } };
            var carousel = new Carousel { ItemsSource = items };

            var carouselAutomationPeer = FrameworkElementAutomationPeer.CreatePeerForElement(carousel) as CarouselAutomationPeer;

            Assert.IsNotNull(carouselAutomationPeer, "Verify that the AutomationPeer is CarouselAutomationPeer.");
            Assert.IsFalse(carouselAutomationPeer.CanSelectMultiple, "Verify that CarouselAutomationPeer.CanSelectMultiple is false.");
            Assert.IsTrue(carouselAutomationPeer.IsSelectionRequired, "Verify that CarouselAutomationPeer.IsSelectionRequired is false.");

            carousel.SetValue(AutomationProperties.NameProperty, automationName);
            Assert.IsTrue(carouselAutomationPeer.GetName().Contains(automationName), "Verify that the UIA name contains the given AutomationProperties.Name of the Carousel.");

            carousel.Name = name;
            Assert.IsTrue(carouselAutomationPeer.GetName().Contains(name), "Verify that the UIA name contains the given Name of the Carousel.");
        }

        public class PhotoDataItem
        {
            public string Title { get; set; }

            public string Category { get; set; }

            public string Thumbnail { get; set; }

            public override string ToString()
            {
                return Title;
            }
        }
    }
}