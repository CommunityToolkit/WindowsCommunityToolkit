// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public partial class Test_ConstrainedBox : VisualUITestBase
    {
        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Normal_AspectHorizontal()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" AspectRatio=""2:1"" Width=""200"">
        <Border HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Background=""Red""/>
      </controls:ConstrainedBox>
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
        public async Task Test_ConstrainedBox_Normal_AspectVertical()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" AspectRatio=""1:2"" Height=""200"">
        <Border HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Background=""Red""/>
      </controls:ConstrainedBox>
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
        public async Task Test_ConstrainedBox_Normal_IntegerWidth()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" AspectRatio=""2"" Height=""100"">
        <Border HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Background=""Red""/>
      </controls:ConstrainedBox>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Check Size
                Assert.AreEqual(2.0, panel.AspectRatio, 0.01, "ApectRatio does not meet expected value of 2.0");

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
        public void Test_ConstrainedBox_AspectRatioParsing_WidthAndHeight()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                AspectRatio ratio = AspectRatio.ConvertToAspectRatio("1.666:1.2");

                Assert.AreEqual(ratio.Width, 1.666);
                Assert.AreEqual(ratio.Height, 1.2);

                // Explicit tests for other culture infos, see https://github.com/CommunityToolkit/WindowsCommunityToolkit/issues/4252
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("it-IT");

                ratio = AspectRatio.ConvertToAspectRatio("1.666:1.2");

                Assert.AreEqual(ratio.Width, 1.666);
                Assert.AreEqual(ratio.Height, 1.2);

                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

                ratio = AspectRatio.ConvertToAspectRatio("1.666:1.2");

                Assert.AreEqual(ratio.Width, 1.666);
                Assert.AreEqual(ratio.Height, 1.2);
            }
            finally
            {
                CultureInfo.CurrentCulture = currentCulture;
            }
        }

        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public void Test_ConstrainedBox_AspectRatioParsing_Ratio()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                AspectRatio ratio = AspectRatio.ConvertToAspectRatio("1.666");

                Assert.AreEqual(ratio.Width, 1.666);
                Assert.AreEqual(ratio.Height, 1);

                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("it-IT");

                ratio = AspectRatio.ConvertToAspectRatio("1.666");

                Assert.AreEqual(ratio.Width, 1.666);
                Assert.AreEqual(ratio.Height, 1);

                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");

                ratio = AspectRatio.ConvertToAspectRatio("1.666");

                Assert.AreEqual(ratio.Width, 1.666);
                Assert.AreEqual(ratio.Height, 1);
            }
            finally
            {
                CultureInfo.CurrentCulture = currentCulture;
            }
        }
    }
}
