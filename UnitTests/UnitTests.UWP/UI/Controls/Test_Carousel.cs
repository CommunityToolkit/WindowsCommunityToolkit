// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Microsoft.Toolkit.Uwp.UI.Automation.Peers;
using Microsoft.Toolkit.Uwp.Extensions;
using Carousel = Microsoft.Toolkit.Uwp.UI.Controls.Carousel;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    [TestCategory("Test_Carousel")]
    public class Test_Carousel : VisualUITestBase
    {
        [TestMethod]
        public async Task ShouldConfigureCarouselAutomationPeerAsync()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                const int selectedIndex = 1;
                var items = new ObservableCollection<PhotoDataItem> { new PhotoDataItem { Title = "Hello" }, new PhotoDataItem { Title = "World" } };
                var carousel = new Carousel { ItemsSource = items };

                await SetTestContentAsync(carousel);

                // Sets the selected item to "World" from the XAML above.
                carousel.SelectedIndex = selectedIndex;

                const string automationName = "MyAutomationPhotoItems";
                const string name = "MyPhotoItems";

                var carouselAutomationPeer =
                    FrameworkElementAutomationPeer.CreatePeerForElement(carousel) as CarouselAutomationPeer;

                Assert.IsNotNull(carouselAutomationPeer, "Verify that the AutomationPeer is CarouselAutomationPeer.");
                Assert.IsFalse(carouselAutomationPeer.CanSelectMultiple, "Verify that CarouselAutomationPeer.CanSelectMultiple is false.");
                Assert.IsTrue(carouselAutomationPeer.IsSelectionRequired, "Verify that CarouselAutomationPeer.IsSelectionRequired is true.");

                carousel.SetValue(AutomationProperties.NameProperty, automationName);
                Assert.IsTrue(carouselAutomationPeer.GetName().Contains(automationName), "Verify that the UIA name contains the given AutomationProperties.Name of the Carousel.");

                carousel.Name = name;
                Assert.IsTrue(carouselAutomationPeer.GetName().Contains(name), "Verify that the UIA name contains the given Name of the Carousel.");

                var carouselItemAutomationPeers = carouselAutomationPeer.GetChildren().Cast<CarouselItemAutomationPeer>().ToList();
                Assert.AreEqual(items.Count,  carouselItemAutomationPeers.Count);

                for (var i = 0; i < carouselItemAutomationPeers.Count; i++)
                {
                    var peer = carouselItemAutomationPeers[i];
                    Assert.AreEqual(i + 1, peer.GetPositionInSet());
                    Assert.AreEqual(items.Count, peer.GetSizeOfSet());
                }

                var selected = carouselItemAutomationPeers.FirstOrDefault(peer => peer.IsSelected);
                Assert.IsNotNull(selected);
                Assert.IsTrue(selected.GetName().Contains(items[selectedIndex].ToString()));
            });
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