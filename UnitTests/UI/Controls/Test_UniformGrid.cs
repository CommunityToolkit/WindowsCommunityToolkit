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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

using Assert = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.Assert;

namespace UnitTests.UI.Controls
{
    [TestClass]
    #pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
    public class Test_UniformGrid
    {
        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetDimensions_AllVisible()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            var children = grid.Children.Select(item => item as FrameworkElement);

            Assert.AreEqual(8, grid.Children.Count());

            var dimensions = UniformGrid.GetDimensions(ref children, 0, 0, 0);
            
            Assert.AreEqual(3, dimensions.rows);
            Assert.AreEqual(3, dimensions.columns);
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetDimensions_FirstColumn()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            var children = grid.Children.Select(item => item as FrameworkElement);

            Assert.AreEqual(8, grid.Children.Count());

            var dimensions = UniformGrid.GetDimensions(ref children, 0, 0, 2);

            Assert.AreEqual(4, dimensions.rows);
            Assert.AreEqual(4, dimensions.columns);
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_Basic()
        {
            var grid = new UniformGrid();

            bool[,] test = new bool[4, 5]
                {
                    { false,  true, false,  true, false },
                    { false,  true,  true,  true, false },
                    { false,  true, false,  true, false },
                    { false, false,  true, false, false },
                };

            var results = UniformGrid.GetFreeSpot(test, 0, false).ToArray();

            var expected = new(int row, int column)[]
                {
                    (0, 0),       (0, 2),       (0, 4),
                    (1, 0),                     (1, 4),
                    (2, 0),       (2, 2),       (2, 4),
                    (3, 0),(3, 1),       (3, 3),(3, 4)
                };

            CollectionAssert.AreEqual(
                expected,
                results, 
                "GetFreeSpot failed.  Expected:\n{0}.\nActual:\n{1}", 
                expected.ToArrayString(), 
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_FirstColumn()
        {
            var grid = new UniformGrid();

            bool[,] test = new bool[4, 5]
                {
                    { true,  false, false,  true, false },
                    { false,  true,  true,  true, false },
                    { false,  true,  true,  true, false },
                    { false, false,  true, false, false },
                };

            var results = UniformGrid.GetFreeSpot(test, 2, false).ToArray();

            var expected = new(int row, int column)[]
                {
                                  (0, 2),       (0, 4),
                    (1, 0),                     (1, 4),
                    (2, 0),                     (2, 4),
                    (3, 0),(3, 1),       (3, 3),(3, 4)
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed FirstColumn.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_Reverse()
        {
            var grid = new UniformGrid();

            bool[,] test = new bool[4, 5]
                {
                    { false, false, false,  true, false },
                    { false,  true,  true, false, false },
                    { true,  false, false,  true, false },
                    { false, false,  true, false, false },
                };

            var results = UniformGrid.GetFreeSpot(test, 0, true).ToArray();

            // right to left, so this should be reversed
            var expected = new(int row, int column)[]
                {
                    (0, 4),       (0, 2),(0, 1),(0, 0),
                    (1, 4),(1, 3),              (1, 0),
                    (2, 4),       (2, 2),(2, 1),
                    (3, 4),(3, 3),       (3, 1),(3, 0)
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed RightToLeft.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_SetupRowDefinitions_AllAutomatic()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            // Normal Grid's don't have any definitions to start
            Assert.AreEqual(0, grid.RowDefinitions.Count);

            //// DO IT AGAIN
            //// This is so we can check that it will behave the same again on another pass.

            // We'll have three rows in this setup
            grid.SetupRowDefinitions(3);

            // We should now have our rows created
            Assert.AreEqual(3, grid.RowDefinitions.Count);

            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var rd = grid.RowDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Height.GridUnitType);

                Assert.AreEqual(1.0, rd.Height.Value);
            }

            // We'll have three rows in this setup
            grid.SetupRowDefinitions(3);

            // We should now have our rows created
            Assert.AreEqual(3, grid.RowDefinitions.Count);

            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                var rd = grid.RowDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Height.GridUnitType);

                Assert.AreEqual(1.0, rd.Height.Value);
            }
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_SetupRowDefinitions_FirstFixed()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <controls:UniformGrid.RowDefinitions>
            <RowDefinition Height=""48""/>
        </controls:UniformGrid.RowDefinitions>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            // We should find our first definition
            Assert.AreEqual(1, grid.RowDefinitions.Count);
            Assert.AreEqual(48, grid.RowDefinitions[0].Height.Value);

            // We'll have three rows in this setup
            grid.SetupRowDefinitions(3);

            // We should now have our rows created
            Assert.AreEqual(3, grid.RowDefinitions.Count);

            var rdo = grid.RowDefinitions[0];
            
            // Did we mark that our row is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Height.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Height.Value);

            // Check that we filled in the other two.
            for (int i = 1; i < grid.RowDefinitions.Count; i++)
            {
                var rd = grid.RowDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Height.GridUnitType);

                Assert.AreEqual(1.0, rd.Height.Value);
            }

            //// DO IT AGAIN
            //// This is so we can check that it will behave the same again on another pass.

            // We'll have three rows in this setup
            grid.SetupRowDefinitions(3);

            // We should now have our rows created
            Assert.AreEqual(3, grid.RowDefinitions.Count);

            rdo = grid.RowDefinitions[0];

            // Did we mark that our row is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Height.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Height.Value);

            // Check that we filled in the other two.
            for (int i = 1; i < grid.RowDefinitions.Count; i++)
            {
                var rd = grid.RowDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Height.GridUnitType);

                Assert.AreEqual(1.0, rd.Height.Value);
            }
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_SetupRowDefinitions_MiddleFixed()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"" Rows=""5"">
        <controls:UniformGrid.RowDefinitions>
            <RowDefinition Height=""48"" controls:UniformGrid.Row=""2""/>
        </controls:UniformGrid.RowDefinitions>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            // We should find our first definition
            Assert.AreEqual(1, grid.RowDefinitions.Count, "Expected to find our row definition.");
            Assert.AreEqual(48, grid.RowDefinitions[0].Height.Value);
            Assert.AreEqual(2, UniformGrid.GetRow(grid.RowDefinitions[0]));

            Assert.AreEqual(5, grid.Rows, "Rows not set to 5");

            // We'll have five rows in this setup
            grid.SetupRowDefinitions(5);

            // We should now have our rows created
            Assert.AreEqual(5, grid.RowDefinitions.Count, "5 RowDefinitions weren't created.");

            // Our original definition should be at index 2
            var rdo = grid.RowDefinitions[2];

            // Did we mark that our row is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));
            Assert.AreEqual(2, UniformGrid.GetRow(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Height.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Height.Value);

            // Check that we filled in the other two.
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                if (i == 2)
                {
                    continue;
                }

                var rd = grid.RowDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Height.GridUnitType);

                Assert.AreEqual(1.0, rd.Height.Value);
            }

            //// DO IT AGAIN
            //// This is so we can check that it will behave the same again on another pass.

            // We'll have five rows in this setup
            grid.SetupRowDefinitions(5);

            // We should now have our rows created
            Assert.AreEqual(5, grid.RowDefinitions.Count);

            // Our original definition should be at index 2
            rdo = grid.RowDefinitions[2];

            // Did we mark that our row is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));
            Assert.AreEqual(2, UniformGrid.GetRow(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Height.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Height.Value);

            // Check that we filled in the other two.
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                if (i == 2)
                {
                    continue;
                }

                var rd = grid.RowDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Height.GridUnitType);

                Assert.AreEqual(1.0, rd.Height.Value);
            }
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_SetupRowDefinitions_MiddleAndEndFixed()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"" Rows=""5"">
        <controls:UniformGrid.RowDefinitions>
            <RowDefinition Height=""48"" controls:UniformGrid.Row=""2""/>
            <RowDefinition Height=""128"" controls:UniformGrid.Row=""4""/>
        </controls:UniformGrid.RowDefinitions>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            // We should find our first definition
            Assert.AreEqual(2, grid.RowDefinitions.Count, "Expected to find two row definitions.");
            Assert.AreEqual(48, grid.RowDefinitions[0].Height.Value);
            Assert.AreEqual(2, UniformGrid.GetRow(grid.RowDefinitions[0]));
            Assert.AreEqual(128, grid.RowDefinitions[1].Height.Value);
            Assert.AreEqual(4, UniformGrid.GetRow(grid.RowDefinitions[1]));

            Assert.AreEqual(5, grid.Rows, "Rows not set to 5");

            // We'll have five rows in this setup
            grid.SetupRowDefinitions(5);

            // We should now have our rows created
            Assert.AreEqual(5, grid.RowDefinitions.Count, "5 RowDefinitions weren't created.");

            // Our original definition should be at index 2
            var rdo = grid.RowDefinitions[2];

            // Did we mark that our row is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));
            Assert.AreEqual(2, UniformGrid.GetRow(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Height.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Height.Value);

            // Our 2nd original definition should be at index 4
            var rdo2 = grid.RowDefinitions[4];

            // Did we mark that our row is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo2));
            Assert.AreEqual(4, UniformGrid.GetRow(rdo2));

            Assert.AreNotEqual(GridUnitType.Star, rdo2.Height.GridUnitType);

            Assert.AreNotEqual(1.0, rdo2.Height.Value);

            // Check that we filled in the other two.
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                if (i == 2 || i == 4)
                {
                    continue;
                }

                var rd = grid.RowDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Height.GridUnitType);

                Assert.AreEqual(1.0, rd.Height.Value);
            }

            //// DO IT AGAIN
            //// This is so we can check that it will behave the same again on another pass.

            // We'll have five rows in this setup
            grid.SetupRowDefinitions(5);

            // We should now have our rows created
            Assert.AreEqual(5, grid.RowDefinitions.Count, "5 RowDefinitions weren't created.");

            // Our original definition should be at index 2
            rdo = grid.RowDefinitions[2];

            // Did we mark that our row is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));
            Assert.AreEqual(2, UniformGrid.GetRow(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Height.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Height.Value);

            // Our 2nd original definition should be at index 4
            rdo2 = grid.RowDefinitions[4];

            // Did we mark that our row is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo2));
            Assert.AreEqual(4, UniformGrid.GetRow(rdo2));

            Assert.AreNotEqual(GridUnitType.Star, rdo2.Height.GridUnitType);

            Assert.AreNotEqual(1.0, rdo2.Height.Value);

            // Check that we filled in the other two.
            for (int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                if (i == 2 || i == 4)
                {
                    continue;
                }

                var rd = grid.RowDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Height.GridUnitType);

                Assert.AreEqual(1.0, rd.Height.Value);
            }
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_SetupColumnDefinitions_AllAutomatic()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            // Normal Grid's don't have any definitions to start
            Assert.AreEqual(0, grid.ColumnDefinitions.Count);

            // We'll have three columns in this setup
            grid.SetupColumnDefinitions(3);

            // We should now have our columns created
            Assert.AreEqual(3, grid.ColumnDefinitions.Count);

            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var cd = grid.ColumnDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(cd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, cd.Width.GridUnitType);

                Assert.AreEqual(1.0, cd.Width.Value);
            }

            //// DO IT AGAIN
            //// This is so we can check that it will behave the same again on another pass.
            
            // We'll have three columns in this setup
            grid.SetupColumnDefinitions(3);

            // We should now have our columns created
            Assert.AreEqual(3, grid.ColumnDefinitions.Count);

            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                var cd = grid.ColumnDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(cd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, cd.Width.GridUnitType);

                Assert.AreEqual(1.0, cd.Width.Value);
            }
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_SetupColumnDefinitions_FirstFixed()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"">
        <controls:UniformGrid.ColumnDefinitions>
            <ColumnDefinition Width=""48""/>
        </controls:UniformGrid.ColumnDefinitions>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            // We should find our first definition
            Assert.AreEqual(1, grid.ColumnDefinitions.Count);
            Assert.AreEqual(48, grid.ColumnDefinitions[0].Width.Value);

            // We'll have three columns in this setup
            grid.SetupColumnDefinitions(3);

            // We should now have our columns created
            Assert.AreEqual(3, grid.ColumnDefinitions.Count);

            var cdo = grid.ColumnDefinitions[0];

            // Did we mark that our column is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(cdo));

            Assert.AreNotEqual(GridUnitType.Star, cdo.Width.GridUnitType);

            Assert.AreNotEqual(1.0, cdo.Width.Value);

            // Check that we filled in the other two.
            for (int i = 1; i < grid.ColumnDefinitions.Count; i++)
            {
                var cd = grid.ColumnDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(cd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, cd.Width.GridUnitType);

                Assert.AreEqual(1.0, cd.Width.Value);
            }

            //// DO IT AGAIN
            //// This is so we can check that it will behave the same again on another pass.

            // We'll have three columns in this setup
            grid.SetupColumnDefinitions(3);

            // We should now have our columns created
            Assert.AreEqual(3, grid.ColumnDefinitions.Count);

            cdo = grid.ColumnDefinitions[0];

            // Did we mark that our column is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(cdo));

            Assert.AreNotEqual(GridUnitType.Star, cdo.Width.GridUnitType);

            Assert.AreNotEqual(1.0, cdo.Width.Value);

            // Check that we filled in the other two.
            for (int i = 1; i < grid.ColumnDefinitions.Count; i++)
            {
                var cd = grid.ColumnDefinitions[i];

                // Check if we've setup our row to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(cd));

                // We need to be using '*' layout for all our rows to be even.
                Assert.AreEqual(GridUnitType.Star, cd.Width.GridUnitType);

                Assert.AreEqual(1.0, cd.Width.Value);
            }
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_SetupColumnDefinitions_MiddleFixed()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"" Columns=""5"">
        <controls:UniformGrid.ColumnDefinitions>
            <ColumnDefinition Width=""48"" controls:UniformGrid.Column=""2""/>
        </controls:UniformGrid.ColumnDefinitions>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            // We should find our first definition
            Assert.AreEqual(1, grid.ColumnDefinitions.Count, "Expected to find our Column definition.");
            Assert.AreEqual(48, grid.ColumnDefinitions[0].Width.Value);
            Assert.AreEqual(2, UniformGrid.GetColumn(grid.ColumnDefinitions[0]));

            Assert.AreEqual(5, grid.Columns, "Columns not set to 5");

            // We'll have five Columns in this setup
            grid.SetupColumnDefinitions(5);

            // We should now have our Columns created
            Assert.AreEqual(5, grid.ColumnDefinitions.Count, "5 ColumnDefinitions weren't created.");

            // Our original definition should be at index 2
            var rdo = grid.ColumnDefinitions[2];

            // Did we mark that our Column is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));
            Assert.AreEqual(2, UniformGrid.GetColumn(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Width.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Width.Value);

            // Check that we filled in the other two.
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                if (i == 2)
                {
                    continue;
                }

                var rd = grid.ColumnDefinitions[i];

                // Check if we've setup our Column to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our Columns to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Width.GridUnitType);

                Assert.AreEqual(1.0, rd.Width.Value);
            }

            //// DO IT AGAIN
            //// This is so we can check that it will behave the same again on another pass.

            // We'll have five Columns in this setup
            grid.SetupColumnDefinitions(5);

            // We should now have our Columns created
            Assert.AreEqual(5, grid.ColumnDefinitions.Count);

            // Our original definition should be at index 2
            rdo = grid.ColumnDefinitions[2];

            // Did we mark that our Column is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));
            Assert.AreEqual(2, UniformGrid.GetColumn(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Width.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Width.Value);

            // Check that we filled in the other two.
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                if (i == 2)
                {
                    continue;
                }

                var rd = grid.ColumnDefinitions[i];

                // Check if we've setup our Column to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our Columns to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Width.GridUnitType);

                Assert.AreEqual(1.0, rd.Width.Value);
            }
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_SetupColumnDefinitions_FirstAndEndFixed()
        {
            var treeroot = XamlReader.Load(@"<Page
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:controls=""using:Microsoft.Toolkit.Uwp.UI.Controls"">
    <controls:UniformGrid x:Name=""UniformGrid"" Columns=""5"">
        <controls:UniformGrid.ColumnDefinitions>
            <ColumnDefinition Width=""48""/>
            <ColumnDefinition Width=""128"" controls:UniformGrid.Column=""4""/>
        </controls:UniformGrid.ColumnDefinitions>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
        <Border/>
    </controls:UniformGrid>
</Page>") as FrameworkElement;

            Assert.IsNotNull(treeroot, "Could not load XAML tree.");

            var grid = treeroot.FindChildByName("UniformGrid") as UniformGrid;

            Assert.IsNotNull(grid, "Could not find UniformGrid in tree.");

            // We should find our first definition
            Assert.AreEqual(2, grid.ColumnDefinitions.Count, "Expected to find two Column definitions.");
            Assert.AreEqual(48, grid.ColumnDefinitions[0].Width.Value);
            Assert.AreEqual(0, UniformGrid.GetColumn(grid.ColumnDefinitions[0]));
            Assert.AreEqual(128, grid.ColumnDefinitions[1].Width.Value);
            Assert.AreEqual(4, UniformGrid.GetColumn(grid.ColumnDefinitions[1]));

            Assert.AreEqual(5, grid.Columns, "Columns not set to 5");

            // We'll have five Columns in this setup
            grid.SetupColumnDefinitions(5);

            // We should now have our Columns created
            Assert.AreEqual(5, grid.ColumnDefinitions.Count, "5 ColumnDefinitions weren't created.");

            // Our original definition should be at index 2
            var rdo = grid.ColumnDefinitions[0];

            // Did we mark that our Column is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));
            Assert.AreEqual(0, UniformGrid.GetColumn(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Width.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Width.Value);

            // Our 2nd original definition should be at index 4
            var rdo2 = grid.ColumnDefinitions[4];

            // Did we mark that our Column is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo2));
            Assert.AreEqual(4, UniformGrid.GetColumn(rdo2));

            Assert.AreNotEqual(GridUnitType.Star, rdo2.Width.GridUnitType);

            Assert.AreNotEqual(1.0, rdo2.Width.Value);

            // Check that we filled in the other two.
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                if (i == 0 || i == 4)
                {
                    continue;
                }

                var rd = grid.ColumnDefinitions[i];

                // Check if we've setup our Column to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our Columns to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Width.GridUnitType);

                Assert.AreEqual(1.0, rd.Width.Value);
            }

            //// DO IT AGAIN
            //// This is so we can check that it will behave the same again on another pass.

            // We'll have five Columns in this setup
            grid.SetupColumnDefinitions(5);

            // We should now have our Columns created
            Assert.AreEqual(5, grid.ColumnDefinitions.Count, "5 ColumnDefinitions weren't created.");

            // Our original definition should be at index 0
            rdo = grid.ColumnDefinitions[0];

            // Did we mark that our Column is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo));
            Assert.AreEqual(0, UniformGrid.GetColumn(rdo));

            Assert.AreNotEqual(GridUnitType.Star, rdo.Width.GridUnitType);

            Assert.AreNotEqual(1.0, rdo.Width.Value);

            // Our 2nd original definition should be at index 4
            rdo2 = grid.ColumnDefinitions[4];

            // Did we mark that our Column is special?
            Assert.AreEqual(false, UniformGrid.GetAutoLayout(rdo2));
            Assert.AreEqual(4, UniformGrid.GetColumn(rdo2));

            Assert.AreNotEqual(GridUnitType.Star, rdo2.Width.GridUnitType);

            Assert.AreNotEqual(1.0, rdo2.Width.Value);

            // Check that we filled in the other two.
            for (int i = 0; i < grid.ColumnDefinitions.Count; i++)
            {
                if (i == 0 || i == 4)
                {
                    continue;
                }

                var rd = grid.ColumnDefinitions[i];

                // Check if we've setup our Column to automatically layout
                Assert.AreEqual(true, UniformGrid.GetAutoLayout(rd));

                // We need to be using '*' layout for all our Columns to be even.
                Assert.AreEqual(GridUnitType.Star, rd.Width.GridUnitType);

                Assert.AreEqual(1.0, rd.Width.Value);
            }
        }
    }
    #pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}