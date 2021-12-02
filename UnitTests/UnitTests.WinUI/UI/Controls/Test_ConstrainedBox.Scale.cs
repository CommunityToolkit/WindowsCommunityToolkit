// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.WinUI;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;

namespace UnitTests.WinUI.UI.Controls
{
    public partial class Test_ConstrainedBox : VisualUITestBase
    {
        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Normal_ScaleX()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:CommunityToolkit.WinUI.UI.Controls"">
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
                Assert.AreEqual(100, child.ActualWidth, 0.01, "Actual width does not meet expected value of 100");
                Assert.AreEqual(200, child.ActualHeight, 0.01, "Actual height does not meet expected value of 200");

                // Check inner Positioning, we do this from the Grid as the ConstainedBox also modifies its own size
                // and is hugging the child.
                var position = grid.CoordinatesTo(child);

                Assert.AreEqual(50, position.X, 0.01, "X position does not meet expected value of 50");
                Assert.AreEqual(0, position.Y, 0.01, "Y position does not meet expected value of 0");
            });
        }

        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Normal_ScaleY()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:CommunityToolkit.WinUI.UI.Controls"">
    <Grid x:Name=""ParentGrid"" Width=""200"" Height=""200"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" ScaleY=""0.5""
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
                Assert.AreEqual(200, child.ActualWidth, 0.01, "Actual width does not meet expected value of 200");
                Assert.AreEqual(100, child.ActualHeight, 0.01, "Actual height does not meet expected value of 100");

                // Check inner Positioning, we do this from the Grid as the ConstainedBox also modifies its own size
                // and is hugging the child.
                var position = grid.CoordinatesTo(child);

                Assert.AreEqual(0, position.X, 0.01, "X position does not meet expected value of 0");
                Assert.AreEqual(50, position.Y, 0.01, "Y position does not meet expected value of 50");
            });
        }
    }
}
