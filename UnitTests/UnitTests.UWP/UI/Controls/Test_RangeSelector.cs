// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTests.UWP.UI.Controls
{
    [TestClass]
    public class Test_RangeSelector : VisualUITestBase
    {
        [TestCategory("Test_RangeSelector")]
        [TestMethod]

        // If Minimum <= RangeMin and RangeMin <= RangeMax and RangeMax <= Maximum and Minimum != Maximum
        [DataRow(0, 10, 90, 100)]
        [DataRow(0, 0, 100, 100)]
        [DataRow(-100, -90, 90, 100)]
        [DataRow(-100, -90, -10, 0)]
        [DataRow(0, 0.2, 0.4, 1)]
        [DataRow(0, 50, 50, 100)]
        [DataRow(0, 0, 0, 100)]
        [DataRow(0, 100, 100, 100)]
        public async Task Test_ValuesStayTheSame(double min, double rangeMin, double rangeMax, double max)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rangeSelector = MakeRangeSelector(min, rangeMin, rangeMax, max);

                await SetTestContentAsync(rangeSelector);

                // Then Minimum, RangeMin, RangeMax, Maximum should keep the vailues they are set to.
                Assert.AreEqual(new TestRecord(min, rangeMin, rangeMax, max, rangeSelector.StepFrequency), new TestRecord(rangeSelector));
            });
        }

        [TestCategory("Test_RangeSelector")]
        [TestMethod]

        // If the RangeMin is set to a number larger than Minimum
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

                // Then RangeMin should get the value of Minimum.
                Assert.AreEqual(new TestRecord(min, min, rangeMax, max, rangeSelector.StepFrequency), new TestRecord(rangeSelector));
            });
        }

        [TestCategory("Test_RangeSelector")]
        [TestMethod]
        
        // If the RangeMax is set to a number larger than Maximum.
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

                // Then RangeMax should get the value of Maximum.
                Assert.AreEqual(new TestRecord(min, rangeMin, max, max, rangeSelector.StepFrequency), new TestRecord(rangeSelector));
            });
        }

        [TestCategory("Test_RangeSelector")]
        [TestMethod]
        
        // If RangeMin is set to a number larger than the RangeMax.
        [DataRow(0, 90, 10, 100)]
        [DataRow(0, 95, 90, 100)]
        [DataRow(0, 10, 5, 100)]
        public async Task Test_RangeMinPastRangeMax(double min, double rangeMin, double rangeMax, double max)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rangeSelector = MakeRangeSelector(min, rangeMin, rangeMax, max);

                await SetTestContentAsync(rangeSelector);

                // Then RangeMin should get the value of RangeMax.
                Assert.AreEqual(new TestRecord(min, rangeMax, rangeMax, max, rangeSelector.StepFrequency), new TestRecord(rangeSelector));
            });
        }

        [TestCategory("Test_RangeSelector")]
        [TestMethod]

        [DataRow(0, 10, 90, 100, 1)]
        [DataRow(0, 10, 90, 100, 5)]
        [DataRow(0, 10, 90, 100, 10)]
        [DataRow(0, 4, 90, 100, 10)]
        [DataRow(0, 5, 90, 100, 10)]
        [DataRow(0, 6, 90, 100, 10)]
        [DataRow(0.5, 0.59, 90, 100, 0.2)]
        [DataRow(0.5, 0.60, 90, 100, 0.2)]
        [DataRow(0.5, 0.61, 90, 100, 0.2)]
        public async Task Test_Step(double min, double rangeMin, double rangeMax, double max, double stepFrequency)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var rangeSelector = MakeRangeSelector(min, rangeMin, rangeMax, max, stepFrequency);

                await SetTestContentAsync(rangeSelector);

                var distances = Distance(min, rangeMin);
                var stepBefore = Math.Floor(distances / stepFrequency);
                var stepAfter = Math.Ceiling(distances / stepFrequency);
                var small = min + (stepBefore * stepFrequency);
                var large = min + (stepAfter * stepFrequency);

                var distanceToSmall = Distance(rangeMin, small);
                var distanceToLarge = Distance(rangeMin, large);

                var expectedMin = distanceToSmall < distanceToLarge ? small : large;


                Assert.AreEqual(new TestRecord(min, expectedMin, rangeMax, max, stepFrequency), new TestRecord(rangeSelector));
            });
        }
        
        [UITestMethod]
        [TestCategory("RangeSelector")]
        public void StepFrequency_SmallValue_RangeMinJumpsToProperStep()
        {
            var selector = new RangeSelector();
            selector.Minimum = 0;
            selector.Maximum = 100;
            selector.StepFrequency = 5;

            selector.RangeStart = 2;
            Assert.AreEqual(selector.RangeStart, 0);

            selector.RangeStart = 4;
            Assert.AreEqual(selector.RangeStart, 5);

            selector.RangeStart = 94;
            Assert.AreEqual(selector.RangeStart, 95);

            selector.RangeStart = 99;
            Assert.AreEqual(selector.RangeStart, 100);
        }

        [UITestMethod]
        [TestCategory("RangeSelector")]
        public void StepFrequency_SmallValue_RangeMaxJumpsToProperStep()
        {
            var selector = new RangeSelector();
            selector.Minimum = 0;
            selector.Maximum = 100;
            selector.StepFrequency = 5;

            selector.RangeEnd = 99;
            Assert.AreEqual(selector.RangeEnd, 100);

            selector.RangeEnd = 94;
            Assert.AreEqual(selector.RangeEnd, 95);

            selector.RangeEnd = 6;
            Assert.AreEqual(selector.RangeEnd, 5);

            selector.RangeEnd = 4;
            Assert.AreEqual(selector.RangeEnd, 5);

            selector.RangeEnd = 1;
            Assert.AreEqual(selector.RangeEnd, 0);
        }

        [UITestMethod]
        [TestCategory("RangeSelector")]
        public void StepFrequency_LargeValue_RangeMinJumpsToProperStep()
        {
            var selector = new RangeSelector();
            selector.Minimum = 0;
            selector.Maximum = 100;
            selector.StepFrequency = 30;

            selector.RangeStart = 2;
            Assert.AreEqual(selector.RangeStart, 0);

            selector.RangeStart = 14;
            Assert.AreEqual(selector.RangeStart, 0);

            selector.RangeStart = 16;
            Assert.AreEqual(selector.RangeStart, 30);

            selector.RangeStart = 29;
            Assert.AreEqual(selector.RangeStart, 30);

            selector.RangeStart = 36;
            Assert.AreEqual(selector.RangeStart, 30);

            selector.RangeStart = 65;
            Assert.AreEqual(selector.RangeStart, 60);

            selector.RangeStart = 86;
            Assert.AreEqual(selector.RangeStart, 90);

            selector.RangeStart = 95;
            Assert.AreEqual(selector.RangeStart, 90);

            selector.RangeStart = 99;
            Assert.AreEqual(selector.RangeStart, 90);

            selector.RangeStart = 100;
            Assert.AreEqual(selector.RangeStart, 90);
        }

        [UITestMethod]
        [TestCategory("RangeSelector")]
        public void StepFrequency_LargeValue_RangeMaxJumpsToProperStep()
        {
            var selector = new RangeSelector();
            selector.Minimum = 0;
            selector.Maximum = 100;
            selector.StepFrequency = 30;

            selector.RangeEnd = 99;
            Assert.AreEqual(selector.RangeEnd, 100);

            selector.RangeEnd = 94;
            Assert.AreEqual(selector.RangeEnd, 100);

            selector.RangeEnd = 78;
            Assert.AreEqual(selector.RangeEnd, 70);

            selector.RangeEnd = 65;
            Assert.AreEqual(selector.RangeEnd, 70);

            selector.RangeEnd = 50;
            Assert.AreEqual(selector.RangeEnd, 40);

            selector.RangeEnd = 38;
            Assert.AreEqual(selector.RangeEnd, 40);

            selector.RangeEnd = 20;
            Assert.AreEqual(selector.RangeEnd, 10);

            selector.RangeEnd = 8;
            Assert.AreEqual(selector.RangeEnd, 10);

            selector.RangeEnd = 1;
            Assert.AreEqual(selector.RangeEnd, 10);

            selector.RangeEnd = 0;
            Assert.AreEqual(selector.RangeEnd, 10);
        }

        private static double Distance(double fst, double snd)
            => Math.Abs(fst - snd);

        private static RangeSelector MakeRangeSelector(double min, double rangeMin, double rangeMax, double max, double? stepFrequency = null)
            => stepFrequency is double sf ?
             new RangeSelector
             {
                 Minimum = min,
                 RangeStart = rangeMin,
                 RangeEnd = rangeMax,
                 Maximum = max,
                 StepFrequency = sf
             }
             :
             new RangeSelector
             {
                 Minimum = min,
                 RangeStart = rangeMin,
                 RangeEnd = rangeMax,
                 Maximum = max
             };

        private struct TestRecord
        {
            public TestRecord(double min, double rangeMin, double rangeMax, double max, double? stepFrequency = null)
            {
                Minimum = min;
                RangeMin = rangeMin;
                RangeMax = rangeMax;
                Maximum = max;
                Stepfrequency = stepFrequency;
            }

            public TestRecord(RangeSelector rangeSelector) 
                : this(rangeSelector.Minimum, rangeSelector.RangeStart, rangeSelector.RangeEnd, rangeSelector.Maximum, rangeSelector.StepFrequency)
            {

            }

            public double Minimum { get; }

            public double RangeMin { get; }

            public double RangeMax { get; }

            public double Maximum { get; }

            public double? Stepfrequency { get; }

            public override string ToString() => JsonSerializer.Serialize(this);
        }
    }
}
