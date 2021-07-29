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
    [TestClass]
    public class Test_ConstrainedBox : VisualUITestBase
    {
        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Normal_Horizontal()
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
                Assert.AreEqual(child.ActualWidth, 200, "Width unexpected");
                Assert.AreEqual(child.ActualHeight, 100, "Height unexpected");
            });
        }

        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Normal_ScaleX()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <Grid x:Name=""ParentGrid"" Width=""200"" Height=""200"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" ScaleX=""0.5""
                               HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"">
        <Border HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Background=""Red""/>
      </controls:ConstrainedBox>
    </Grid>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var grid = treeRoot.FindChild("ParentGrid") as Grid;

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Force Layout calculations
                panel.UpdateLayout();

                var child = panel.Content as Border;

                Assert.IsNotNull(child, "Could not find inner Border");

                // Check Size
                Assert.AreEqual(child.ActualWidth, 100);
                Assert.AreEqual(child.ActualHeight, 200);

                // Check inner Positioning, we do this from the Grid as the ConstainedBox also modifies its own size
                // and is hugging the child.
                var position = grid.CoordinatesTo(child);

                Assert.AreEqual(position.X, 50);
                Assert.AreEqual(position.Y, 0);
            });
        }
    }
}