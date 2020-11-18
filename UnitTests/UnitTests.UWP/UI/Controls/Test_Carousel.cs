// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
            var items = new ObservableCollection<PhotoDataItem> { new PhotoDataItem { Title = "Hello" }, new PhotoDataItem { Title = "World" } };
            var carousel = new Carousel { ItemsSource = items };

            var carouselPeer = FrameworkElementAutomationPeer.CreatePeerForElement(carousel) as CarouselAutomationPeer;

            Assert.IsNotNull(carouselPeer, "Verify that the AutomationPeer is CarouselAutomationPeer.");
            Assert.IsFalse(carouselPeer.CanSelectMultiple, "Verify that CarouselAutomationPeer.CanSelectMultiple is false.");
            Assert.IsFalse(carouselPeer.IsSelectionRequired, "Verify that CarouselAutomationPeer.IsSelectionRequired is false.");

            Assert.IsTrue(carouselPeer.GetName().Contains(nameof(Carousel)), "Verify that the UIA name contains the class name of the Carousel.");
            carousel.Name = "TextItems";
            Assert.IsTrue(carouselPeer.GetName().Contains(carousel.Name), "Verify that the UIA name contains the given name of the Carousel.");
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