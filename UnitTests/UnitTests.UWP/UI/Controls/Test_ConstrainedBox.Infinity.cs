// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UWP.UI.Controls
{
    /// <summary>
    /// These tests check for cases where one of the bounds provided by the parent panel is infinite.
    /// </summary>
    public partial class Test_ConstrainedBox : VisualUITestBase
    {
        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_AllInfinite_AspectWidthFallback()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                // Even though we constrain the size of the ScrollViewer, it initially reports infinite measure
                // which is what we want to test. We constrain the dimension so that we have a specific value
                // that we can measure against. If we instead restrained the inner Border, it'd just set
                // it's side within the constrained space of the parent layout...
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <ScrollViewer HorizontalScrollMode=""Enabled"" HorizontalScrollBarVisibility=""Visible""
                  VerticalScrollMode=""Enabled"" VerticalScrollBarVisibility=""Visible""
                  Width=""200"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" AspectRatio=""2:1"">
        <Border HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Background=""Red""/>
      </controls:ConstrainedBox>
    </ScrollViewer>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Force Layout calculations
                panel.UpdateLayout();

                var child = panel.Content as Border;

                Assert.IsNotNull(child, "Could not find inner Border");

                // Check Size
                Assert.AreEqual(200, child.ActualWidth, 0.01, "Actual width does not meet expected value of 200");
                Assert.AreEqual(100, child.ActualHeight, 0.01, "Actual height does not meet expected value of 100");
            });
        }

        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_AllInfinite_AspectHeightFallback()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <ScrollViewer HorizontalScrollMode=""Enabled"" HorizontalScrollBarVisibility=""Visible""
                  VerticalScrollMode=""Enabled"" VerticalScrollBarVisibility=""Visible""
                  Height=""200"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" AspectRatio=""1:2"">
        <Border HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Background=""Red""/>
      </controls:ConstrainedBox>
    </ScrollViewer>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Force Layout calculations
                panel.UpdateLayout();

                var child = panel.Content as Border;

                Assert.IsNotNull(child, "Could not find inner Border");

                // Check Size
                Assert.AreEqual(100, child.ActualWidth, 0.01, "Actual width does not meet expected value of 100");
                Assert.AreEqual(200, child.ActualHeight, 0.01, "Actual height does not meet expected value of 200");
            });
        }

        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_AllInfinite_AspectBothFallback()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <ScrollViewer x:Name=""ScrollArea""
                  HorizontalScrollMode=""Enabled"" HorizontalScrollBarVisibility=""Visible""
                  VerticalScrollMode=""Enabled"" VerticalScrollBarVisibility=""Visible"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" AspectRatio=""1:2"" UseLayoutRounding=""False"">
        <Border HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Background=""Red""/>
      </controls:ConstrainedBox>
    </ScrollViewer>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var scroll = treeRoot.FindChild("ScrollArea") as ScrollViewer;

                Assert.IsNotNull(scroll, "Could not find ScrollViewer in tree.");

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Force Layout calculations
                panel.UpdateLayout();

                var child = panel.Content as Border;

                Assert.IsNotNull(child, "Could not find inner Border");

                var width = scroll.ActualHeight / 2;

                // Check Size
                Assert.AreEqual(width, child.ActualWidth, 0.01, "Actual width does not meet expected value of " + width);
                Assert.AreEqual(scroll.ActualHeight, child.ActualHeight, 0.01, "Actual height does not meet expected value of " + scroll.ActualHeight);
            });
        }
    }
}
