// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Linq;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace UnitTests.UI.Controls
{
    [TestClass]
    #pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
    public class Test_UniformGrid_AutoLayout
    {
        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_AutoLayout_FixedElementSingle()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border Grid.Row=""1"" Grid.Column=""1""/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            var expected = new(int row, int col)[]
            {
                (0, 0),
                (1, 1),
                (0, 1),
                (0, 2),
                (1, 0),
                (1, 2),
                (2, 0),
                (2, 1)
            };

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            var children = grid.Children.Select(item => item as FrameworkElement).ToArray();

            Assert.AreEqual(8, grid.Children.Count);

            grid.Measure(new Size(1000, 1000));

            // Check all children are in expected places.
            for (int i = 0; i < children.Count(); i++)
            {
                if (expected[i].row == 1 && expected[i].col == 1)
                {
                    // Check our fixed item isn't set to auto-layout.
                    Assert.AreEqual(false, UniformGrid.GetAutoLayout(children[i]));
                }

                Assert.AreEqual(expected[i].row, Grid.GetRow(children[i]));
                Assert.AreEqual(expected[i].col, Grid.GetColumn(children[i]));
            }
        }

        /// <summary>
        /// Note: This one particular special-case scenario requires 16299 for the <see cref="MarkupExtension"/>.
        /// </summary>
        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_AutoLayout_FixedElementZeroZeroSpecial()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:ex=""using:Microsoft.Toolkit.Uwp.UI.Extensions""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <!-- Since Grid Row/Columns are 0 by default, we need to also add 
             AutoLayout False here as well to get the desired behavior,
             Otherwise we can't tell it apart from the other items. -->
        <Border Grid.Row=""0"" Grid.Column=""0"" controls:UniformGrid.AutoLayout=""{ex:NullableBool Value=False}""/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            var expected = new(int row, int col)[]
            {
                (0, 1),
                (0, 2),
                (1, 0),
                (1, 1),
                (1, 2),
                (2, 0),
                (0, 0),
                (2, 1)
            };

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            var children = grid.Children.Select(item => item as FrameworkElement).ToArray();

            Assert.AreEqual(8, grid.Children.Count);

            grid.Measure(new Size(1000, 1000));

            // Check all children are in expected places.
            for (int i = 0; i < children.Count(); i++)
            {
                if (expected[i].row == 0 && expected[i].col == 0)
                {
                    // Check our fixed item isn't set to auto-layout.
                    Assert.AreEqual(false, UniformGrid.GetAutoLayout(children[i]));
                }

                Assert.AreEqual(expected[i].row, Grid.GetRow(children[i]));
                Assert.AreEqual(expected[i].col, Grid.GetColumn(children[i]));
            }
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_AutoLayout_FixedElementSquare()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border Grid.Row=""1"" Grid.Column=""1"" Grid.RowSpan=""2"" Grid.ColumnSpan=""2""/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            var expected = new(int row, int col)[]
            {
                (0, 0),
                (1, 1),
                (0, 1),
                (0, 2),
                (0, 3),
                (1, 0),
                (1, 3),
                (2, 0),
                (2, 3)
            };

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            var children = grid.Children.Select(item => item as FrameworkElement).ToArray();

            Assert.AreEqual(9, grid.Children.Count);

            grid.Measure(new Size(1000, 1000));

            // Check all children are in expected places.
            for (int i = 0; i < children.Count(); i++)
            {
                if (expected[i].row == 1 && expected[i].col == 1)
                {
                    // Check our fixed item isn't set to auto-layout.
                    Assert.AreEqual(false, UniformGrid.GetAutoLayout(children[i]));
                }

                Assert.AreEqual(expected[i].row, Grid.GetRow(children[i]));
                Assert.AreEqual(expected[i].col, Grid.GetColumn(children[i]));
            }
        }
    }
    #pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}