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
    /// These tests check whether the inner alignment of the box within it's parent works as expected.
    /// </summary>
    public partial class Test_ConstrainedBox : VisualUITestBase
    {
        // For this test we're testing within the confines of a 200x200 box to position a contrained
        // 50x100 element in all the different alignment combinations.
        [TestCategory("ConstrainedBox")]
        [TestMethod]
        [DataRow("Left", 0, "Center", 50, DisplayName = "LeftCenter")]
        [DataRow("Left", 0, "Top", 0, DisplayName = "LeftTop")]
        [DataRow("Center", 75, "Top", 0, DisplayName = "CenterTop")]
        [DataRow("Right", 150, "Top", 0, DisplayName = "RightTop")]
        [DataRow("Right", 150, "Center", 50, DisplayName = "RightCenter")]
        [DataRow("Right", 150, "Bottom", 100, DisplayName = "RightBottom")]
        [DataRow("Center", 75, "Bottom", 100, DisplayName = "CenterBottom")]
        [DataRow("Left", 0, "Bottom", 100, DisplayName = "LeftBottom")]
        [DataRow("Center", 75, "Center", 50, DisplayName = "CenterCenter")]
        public async Task Test_ConstrainedBox_Alignment_Aspect(string horizontalAlignment, int expectedLeft, string verticalAlignment, int expectedTop)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@$"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <Grid x:Name=""ParentGrid""
          Width=""200"" Height=""200"">
      <controls:ConstrainedBox x:Name=""ConstrainedBox"" AspectRatio=""1:2"" MaxHeight=""100""
                               UseLayoutRounding=""False""
                               HorizontalAlignment=""{horizontalAlignment}""
                               VerticalAlignment=""{verticalAlignment}"">
        <Border HorizontalAlignment=""Stretch"" VerticalAlignment=""Stretch"" Background=""Red""/>
      </controls:ConstrainedBox>
    </Grid>
</Page>") as FrameworkElement;

                Assert.IsNotNull(treeRoot, "Could not load XAML tree.");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var grid = treeRoot.FindChild("ParentGrid") as Grid;

                Assert.IsNotNull(grid, "Could not find the ParentGrid in tree.");

                var panel = treeRoot.FindChild("ConstrainedBox") as ConstrainedBox;

                Assert.IsNotNull(panel, "Could not find ConstrainedBox in tree.");

                // Force Layout calculations
                panel.UpdateLayout();

                var child = panel.Content as Border;

                Assert.IsNotNull(child, "Could not find inner Border");

                // Check Size
                Assert.AreEqual(50, child.ActualWidth, 0.01, "Actual width does not meet expected value of 50");
                Assert.AreEqual(100, child.ActualHeight, 0.01, "Actual height does not meet expected value of 100");

                // Check inner Positioning, we do this from the Grid as the ConstainedBox also modifies its own size
                // and is hugging the child.
                var position = grid.CoordinatesTo(child);

                Assert.AreEqual(expectedLeft, position.X, 0.01, "X position does not meet expected value of 0");
                Assert.AreEqual(expectedTop, position.Y, 0.01, "Y position does not meet expected value of 50");
            });
        }
    }
}
