// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Windows.Apps.Test.Foundation;
using Microsoft.Windows.Apps.Test.Foundation.Controls;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Common;
using Windows.UI.Xaml.Tests.MUXControls.InteractionTests.Infra;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Dynamic;
using System;

#if USING_TAEF
using WEX.Logging.Interop;
using WEX.TestExecution;
using WEX.TestExecution.Markup;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace UITests.Tests
{

    [TestClass]
    public class GridSplitterTest : UITestBase
    {
        [ClassInitialize]
        [TestProperty("RunAs", "User")]
        [TestProperty("Classification", "ScenarioTestSuite")]
        [TestProperty("Platform", "Any")]
        public static void ClassInitialize(TestContext testContext)
        {
            TestEnvironment.Initialize(testContext, WinUICsUWPSampleApp);
        }

        [TestMethod]
        [TestPage("GridSplitterTestPage")]
        public async Task TestGridSplitterDragHorizontalAsync()
        {
            var amount = 50;
            var tolerance = 10;

            var grid = FindElement.ByName("GridSplitterRoot");
            var gridSplitter = FindElement.ById("GridSplitterHorizontal");
            var box = FindElement.ByName("TopLeftBox");

            Verify.IsNotNull(grid, "Can't find GridSplitterRoot");
            Verify.IsNotNull(gridSplitter, "Can't find Horizontal GridSplitter");
            Verify.IsNotNull(box, "Can't find box");

            var width = box.BoundingRectangle.Width;

            ColumnDefinition columnDefinitionStart = (await VisualTreeHelper.FindElementPropertyAsync<List<ColumnDefinition>>("GridSplitterRoot", "ColumnDefinitions"))?.FirstOrDefault();

            Verify.IsNotNull(columnDefinitionStart, "Couldn't retrieve Column Definition");

            // Drag to the Left
            InputHelper.DragDistance(gridSplitter, amount, Direction.West, 1000);

            Wait.ForMilliseconds(1050);
            Wait.ForIdle();

            ColumnDefinition columnDefinitionEnd = (await VisualTreeHelper.FindElementPropertyAsync<List<ColumnDefinition>>("GridSplitterRoot", "ColumnDefinitions"))?.FirstOrDefault();

            Wait.ForIdle();

            Verify.IsTrue(Math.Abs(columnDefinitionStart.ActualWidth - amount - columnDefinitionEnd.ActualWidth) <= tolerance, $"ColumnDefinition not in range expected {columnDefinitionStart.ActualWidth - amount} was {columnDefinitionEnd.ActualWidth}");

            Verify.IsTrue(Math.Abs(width - amount - box.BoundingRectangle.Width) <= tolerance, $"Bounding box not in range expected {width - amount} was {box.BoundingRectangle.Width}.");
        }

        [TestMethod]
        [TestPage("GridSplitterTestPage")]
        public async Task TestGridSplitterDragHorizontalPastMinimumAsync()
        {
            var amount = 150;

            var gridSplitter = FindElement.ById("GridSplitterHorizontal");

            Verify.IsNotNull(gridSplitter, "Can't find Horizontal GridSplitter");

            // Drag to the Left
            InputHelper.DragDistance(gridSplitter, amount, Direction.West, 1000);

            Wait.ForMilliseconds(1050);
            Wait.ForIdle();

            ColumnDefinition columnDefinitionEnd = (await VisualTreeHelper.FindElementPropertyAsync<List<ColumnDefinition>>("GridSplitterRoot", "ColumnDefinitions"))?.FirstOrDefault();

            Wait.ForIdle();

            Verify.AreEqual(columnDefinitionEnd.MinWidth, columnDefinitionEnd.ActualWidth, "Column was not the minimum size expected.");
        }

        private class ColumnDefinition
        {
            public GridLength Width { get; set; }

            public double ActualWidth { get; set; }

            public double MinWidth { get; set; }

            public double MaxWidth { get; set; }
        }

        private class GridLength
        {
            public int GridUnitType { get; set; } // 0 Auto, 1 Pixel, 2 Star

            public double Value { get; set; }
        }
    }
}
