// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Extensions;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_LogicalTreeExtensions : VisualUITestBase
    {
        [TestCategory("LogicalTree")]
        [TestMethod]
        public async Task Test_LogicalTree_FindParent_Exists()
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var treeRoot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    <Grid>
        <Grid> <!-- Target -->
            <Border/>
            <Border>
                <TextBlock/> <!-- Starting Point -->
            </Border>
        </Grid>
    </Grid>
</Page>") as Page;

                // Test Setup
                Assert.IsNotNull(treeRoot, "XAML Failed to Load");

                // Initialize Visual Tree
                await SetTestContentAsync(treeRoot);

                var outerGrid = treeRoot.Content as Grid;

                Assert.IsNotNull(outerGrid, "Couldn't find Page content.");

                var targetGrid = outerGrid.Children.FirstOrDefault() as Grid;
                Assert.IsNotNull(targetGrid, "Couldn't find Target Grid");
                Assert.AreEqual(2, targetGrid.Children.Count, "Grid doesn't have right number of children.");

                var secondBorder = targetGrid.Children[1] as Border;
                Assert.IsNotNull(secondBorder, "Border not found.");

                var startingPoint = secondBorder.Child as FrameworkElement;
                Assert.IsNotNull(startingPoint, "Could not find starting element.");

                // Main Test
                var grid = startingPoint.FindParent<Grid>();

                Assert.IsNotNull(grid, "Expected to find Grid");
                Assert.AreEqual(targetGrid, grid, "Grid didn't match expected.");
            });
        }
    }
}
