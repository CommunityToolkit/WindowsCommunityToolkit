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
    /// These tests check for the various values which can be coerced and changed if out of bounds for each property.
    /// </summary>
    public partial class Test_ConstrainedBox : VisualUITestBase
    {
        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Coerce_Scale()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" ScaleX=""0.5"" ScaleY=""0.5""/>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Check Size
                Assert.AreEqual(0.5, panel.ScaleX, 0.01, "ScaleX does not meet expected initial value of 0.5");
                Assert.AreEqual(0.5, panel.ScaleY, 0.01, "ScaleY does not meet expected initial value of 0.5");

                // Change values now to be invalid
                panel.ScaleX = double.NaN;
                panel.ScaleY = double.NaN;

                Assert.AreEqual(1.0, panel.ScaleX, 0.01, "ScaleX does not meet expected value of 1.0 after change.");
                Assert.AreEqual(1.0, panel.ScaleY, 0.01, "ScaleY does not meet expected value of 1.0 after change.");

                // Change values now to be invalid
                panel.ScaleX = double.NegativeInfinity;
                panel.ScaleY = double.NegativeInfinity;

                Assert.AreEqual(0.0, panel.ScaleX, 0.01, "ScaleX does not meet expected value of 0.0 after change.");
                Assert.AreEqual(0.0, panel.ScaleY, 0.01, "ScaleY does not meet expected value of 0.0 after change.");

                // Change values now to be invalid
                panel.ScaleX = double.PositiveInfinity;
                panel.ScaleY = double.PositiveInfinity;

                Assert.AreEqual(1.0, panel.ScaleX, 0.01, "ScaleX does not meet expected value of 1.0 after change.");
                Assert.AreEqual(1.0, panel.ScaleY, 0.01, "ScaleY does not meet expected value of 1.0 after change.");
            });
        }

        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Coerce_Multiple()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" MultipleX=""32"" MultipleY=""32""/>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Check Size
                Assert.AreEqual(32, panel.MultipleX, "MultipleX does not meet expected value of 32");
                Assert.AreEqual(32, panel.MultipleY, "MultipleY does not meet expected value of 32");

                // Change values now to be invalid
                panel.MultipleX = 0;
                panel.MultipleY = int.MinValue;

                Assert.AreEqual(DependencyProperty.UnsetValue, panel.ReadLocalValue(ConstrainedBox.MultipleXProperty), "MultipleX does not meet expected value of UnsetValue after change.");
                Assert.AreEqual(DependencyProperty.UnsetValue, panel.ReadLocalValue(ConstrainedBox.MultipleYProperty), "MultipleY does not meet expected value of UnsetValue after change.");
            });
        }

        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Coerce_AspectRatio()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" AspectRatio=""1.25:3.5""/>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Check Size
                Assert.AreEqual(1.25 / 3.5, panel.AspectRatio, 0.01, "ApectRatio does not meet expected value of 1.25/3.5");

                // Change values now to be invalid
                panel.AspectRatio = -1;

                Assert.AreEqual(DependencyProperty.UnsetValue, panel.ReadLocalValue(ConstrainedBox.AspectRatioProperty), "AspectRatio does not meet expected value of UnsetValue after change.");
            });
        }
    }
}
