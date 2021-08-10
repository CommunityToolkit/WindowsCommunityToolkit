// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.UWP.UI.Controls
{
    /// <summary>
    /// These tests check multiple constraints are applied together in the correct order.
    /// </summary>
    public partial class Test_ConstrainedBox : VisualUITestBase
    {
        [TestCategory("ConstrainedBox")]
        [TestMethod]
        public async Task Test_ConstrainedBox_Combined_All()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                // We turn LayoutRounding off as we're doing between pixel calculation here to test.
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <Grid x:Name=""ParentGrid"" Width=""100"" Height=""100"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" ScaleX=""0.9"" ScaleY=""0.9""
                               MultipleX=""32"" MultipleY=""32""
                               AspectRatio=""3:1""
                               UseLayoutRounding=""False""
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
                Assert.AreEqual(64, child.ActualWidth, 0.01, "Actual width does not meet expected value of 64");
                Assert.AreEqual(21.333, child.ActualHeight, 0.01, "Actual height does not meet expected value of 21.33");

                // Check inner Positioning, we do this from the Grid as the ConstainedBox also modifies its own size
                // and is hugging the child.
                var position = grid.CoordinatesTo(child);

                Assert.AreEqual(18, position.X, 0.01, "X position does not meet expected value of 18");
                Assert.AreEqual(39.333, position.Y, 0.01, "Y position does not meet expected value of 39.33");

                // Update Aspect Ratio and Re-check
                panel.AspectRatio = new AspectRatio(1, 3);

                // Wait to ensure we've redone layout
                await CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() =>
                {
                    // Check Size
                    Assert.AreEqual(21.333, child.ActualWidth, 0.01, "Actual width does not meet expected value of 21.33");
                    Assert.AreEqual(64, child.ActualHeight, 0.01, "Actual height does not meet expected value of 64");

                    // Check inner Positioning, we do this from the Grid as the ConstainedBox also modifies its own size
                    // and is hugging the child.
                    position = grid.CoordinatesTo(child);

                    Assert.AreEqual(39.333, position.X, 0.01, "X position does not meet expected value of 39.33");
                    Assert.AreEqual(18, position.Y, 0.01, "Y position does not meet expected value of 18");
                });
            });
        }
    }
}
