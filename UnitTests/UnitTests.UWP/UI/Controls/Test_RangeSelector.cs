// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public class Test_RangeSelector : VisualUITestBase
    {
        [TestCategory("Test_RangeSelector")]
        [TestMethod]
        [DataRow(0, 10, 90, 100)]
        [DataRow(0, 0, 100, 100)]
        [DataRow(-100, -90, 90, 100)]
        [DataRow(-100, -90, -10, 0)]
        [DataRow(0, 0.2, 0.4, 1)]
        public async Task Test_ValuesStayTheSame(double min, double rangeMin, double rangeMax, double max)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rangeSelector = MakeRangeSelector(min, rangeMin, rangeMax, max);

                await SetTestContentAsync(rangeSelector);

                Assert.AreEqual(new TestRecord(min, rangeMin, rangeMax, max), new TestRecord(rangeSelector));
            });
        }

        [TestCategory("Test_RangeSelector")]
        [TestMethod]
        [DataRow(0, -10, 90, 100)]
        [DataRow(-100, -110, 90, 100)]
        [DataRow(20, 10, 90, 100)]
        [DataRow(20, 20, 90, 100)]
        [DataRow(0.2, 0.1, 0.4, 1)]
        public async Task Test_RangeMinHitMin(double min, double rangeMin, double rangeMax, double max)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rangeSelector = MakeRangeSelector(min, rangeMin, rangeMax, max);

                await SetTestContentAsync(rangeSelector);

                Assert.AreEqual(new TestRecord(min, min, rangeMax, max), new TestRecord(rangeSelector));
            });
        }

        [TestCategory("Test_RangeSelector")]
        [TestMethod]
        [DataRow(0, 10, 110, 100)]
        [DataRow(-100, -90, 110, 100)]
        [DataRow(-100, -90, 10, 0)]
        [DataRow(-100, -90, 10, -10)]
        [DataRow(-100, -90, 0, -10)]
        [DataRow(0, 0.1, 1.5, 1.1)]
        public async Task Test_RangeMaxHitMax(double min, double rangeMin, double rangeMax, double max)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rangeSelector = MakeRangeSelector(min, rangeMin, rangeMax, max);

                await SetTestContentAsync(rangeSelector);

                Assert.AreEqual(new TestRecord(min, rangeMin, max, max), new TestRecord(rangeSelector));
            });
        }

        private static RangeSelector MakeRangeSelector(double min, double rangeMin, double rangeMax, double max)
            => new RangeSelector
            {
                Minimum = min,
                RangeMin = rangeMin,
                RangeMax = rangeMax,
                Maximum = max
            };

        private struct TestRecord
        {
            public TestRecord(double min, double rangeMin, double rangeMax, double max)
            {
                Minimum = min;
                RangeMin = rangeMin;
                RangeMax = rangeMax;
                Maximum = max;
            }

            public TestRecord(RangeSelector rangeSelector) 
                : this(rangeSelector.Minimum, rangeSelector.RangeMin, rangeSelector.RangeMax, rangeSelector.Maximum)
            {

            }

            public double Minimum { get; }

            public double RangeMin { get; }

            public double RangeMax { get; }

            public double Maximum { get; }

            public override string ToString() => JsonSerializer.Serialize(this);
        }
    }
}
