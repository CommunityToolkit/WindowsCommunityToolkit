// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UnitTests.UWP.UI.Triggers
{
    [TestClass]
    [TestCategory("Test_ControlSizeTrigger")]
    public class Test_ControlSizeTrigger : VisualUITestBase
    {
        [DataTestMethod]
        [DataRow(450, 450, true)]
        [DataRow(400, 400, true)]
        [DataRow(500, 500, false)]
        [DataRow(399, 400, false)]
        [DataRow(400, 399, false)]
        public async Task ControlSizeTriggerTest(double width, double height, bool expectedResult)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                Grid grid = CreateGrid(width, height);
                await SetTestContentAsync(grid);
                var trigger = new ControlSizeTrigger();

                trigger.TargetElement = grid;
                trigger.MaxHeight = 500;
                trigger.MinHeight = 400;
                trigger.MaxWidth = 500;
                trigger.MinWidth = 400;

                Assert.AreEqual(expectedResult, trigger.IsActive);
            });
        }

        [DataTestMethod]
        [DataRow(400, 400, true)]
        [DataRow(400, 399, false)]
        public async Task ControlSizeMinHeightTriggerTest(double width, double height, bool expectedResult)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                Grid grid = CreateGrid(width, height);
                await SetTestContentAsync(grid);
                var trigger = new ControlSizeTrigger();

                trigger.TargetElement = grid;
                trigger.MinHeight = 400;

                Assert.AreEqual(expectedResult, trigger.IsActive);
            });
        }

        [DataTestMethod]
        [DataRow(399, 400, false)]
        [DataRow(400, 400, true)]
        public async Task ControlSizeMinWidthTriggerTest(double width, double height, bool expectedResult)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                Grid grid = CreateGrid(width, height);
                await SetTestContentAsync(grid);
                var trigger = new ControlSizeTrigger();

                trigger.TargetElement = grid;
                trigger.MinWidth = 400;

                Assert.AreEqual(expectedResult, trigger.IsActive);
            });
        }

        [DataTestMethod]
        [DataRow(450, 450, false)]
        [DataRow(450, 449, true)]
        public async Task ControlSizeMaxHeightTriggerTest(double width, double height, bool expectedResult)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                Grid grid = CreateGrid(width, height);
                await SetTestContentAsync(grid);
                var trigger = new ControlSizeTrigger();

                trigger.TargetElement = grid;
                trigger.MaxHeight = 450;

                Assert.AreEqual(expectedResult, trigger.IsActive);
            });
        }

        [DataTestMethod]
        [DataRow(450, 450, false)]
        [DataRow(449, 450, true)]
        public async Task ControlSizeMaxWidthTriggerTest(double width, double height, bool expectedResult)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                Grid grid = CreateGrid(width, height);
                await SetTestContentAsync(grid);
                var trigger = new ControlSizeTrigger();

                trigger.TargetElement = grid;
                trigger.MaxWidth = 450;

                Assert.AreEqual(expectedResult, trigger.IsActive);
            });
        }

        private Grid CreateGrid(double width, double height)
        {
            var grid = new Grid()
            {
                Height = height,
                Width = width
            };

            return grid;
        }
    }
}
