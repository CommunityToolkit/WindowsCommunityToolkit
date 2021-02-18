// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls""
    xmlns:data=""using:UnitTests.UWP.UI.Controls"">
    <Grid>
        <controls:Carousel InvertPositive=""True"">
            <controls:Carousel.EasingFunction>
                <CubicEase EasingMode=""EaseOut"" />
            </controls:Carousel.EasingFunction>

            <controls:CarouselItem>
                Hello
            </controls:CarouselItem>
            <controls:CarouselItem>
                World
            </controls:CarouselItem>
         </controls:Carousel>
    </Grid>
</Page>") as Page;

                // This is based on the XAML above.
                var expectedNumItems = 2;
                var expectedSelectedItem = "World";

                Assert.IsNotNull(treeRoot, "XAML for test failed to load");

                await SetTestContentAsync(treeRoot);

                var outerGrid = treeRoot.Content as Grid;

                Assert.IsNotNull(outerGrid, "Couldn't find Page content.");

                var carousel = outerGrid.Children.FirstOrDefault() as Carousel;

                Assert.IsNotNull(carousel, "Couldn't find Target Carousel.");

                // Sets the selected item to "World" from the XAML above.
                carousel.SelectedIndex = 1;

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
                Assert.AreEqual(expectedNumItems, carouselItemAutomationPeers.Count);

                for (var i = 0; i < carouselItemAutomationPeers.Count; i++)
                {
                    var peer = carouselItemAutomationPeers[i];
                    Assert.AreEqual(i + 1, peer.GetPositionInSet());
                    Assert.AreEqual(expectedNumItems, peer.GetSizeOfSet());
                }

                var selected = carouselItemAutomationPeers.FirstOrDefault(peer => peer.IsSelected);
                Assert.IsNotNull(selected);
                Assert.IsTrue(selected.GetName().Contains(expectedSelectedItem));
            });
        }
    }
}