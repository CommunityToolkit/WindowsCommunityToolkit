// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace UnitTests.UI.Controls
{
    [TestClass]
    public class Test_RangeSelector : VisualUITestBase
    {
#pragma warning disable SA1008, SA1025, SA1021
        [TestCategory("Initialize")]
        [TestMethod]

        // If
        // Minimum < Maximum
        //
        // Then
        // Minimum dose not change
        // Maximum dose not change

        // If
        // Minimum < Maximum
        // RangeStart >= Minimum
        // RangeStart <= Maximum
        // RangeStart <= RangeEnd
        //
        // Then
        // RangeStart does not change

        // If
        // Minimum < Maximum
        // RangeEnd >= Minimum
        // RangeEnd <= Maximum
        // RangeEnd >= RangeStart
        //
        // Then
        // RangeEnd dose not change
        // Input:Start End   Expected:Start End
        [DataRow(    0,  100,             0,  100, DisplayName = "Minimum == RangeStart  < RangeEnd == Maximum")]
        [DataRow(   10,   90,            10,   90, DisplayName = "Minimum  < RangeStart  < RangeEnd  < Maximum")]
        [DataRow(   50,   50,            50,   50, DisplayName = "Minimum  < RangeStart == RangeEnd  < Maximum")]
        [DataRow(    0,   90,             0,   90, DisplayName = "Minimum == RangeStart  < RangeEnd  < Maximum")]
        [DataRow(    0,    0,             0,    0, DisplayName = "Minimum == RangeStart == RangeEnd  < Maximum")]
        [DataRow(   10,  100,            10,  100, DisplayName = "Minimum  < RangeStart  < RangeEnd == Maximum")]
        [DataRow(  100,  100,           100,  100, DisplayName = "Minimum  < RangeStart == RangeEnd == Maximum")]

        // If
        // Minimum < Maximum
        // RangeEnd <= Maximum
        // RangeEnd >= Minimum
        // RangeStart >= Minimum
        // RangeStart >= RangeEnd
        //
        // Then
        // RangeStart will be RangeEnd
        [DataRow(   90,   10,            10,   10, DisplayName = "Minimum  < RangeStart  > RangeEnd  < Maximum")]
        [DataRow(  110,   10,            10,   10, DisplayName = "Minimum  < RangeStart  > RangeEnd  < Maximum, RangeStart > Maximum")]

        // If
        // Minimum < Maximum
        // RangeStart <= Minimum
        //
        // Then
        // RangeStart will be Minimum

        // If
        // Minimum < Maximum
        // RangeEnd <= Minimum
        //
        // Then
        // RangeEnd will be Minimum

        // If 
        // Minimum < Maximum
        // RangeEnd <= Minimum
        // RangeStart >= RangeEnd
        //
        // Then
        // RangeEnd will be Minimum
        // RangeStart will be Minimum
        [DataRow(  -50,  -50,             0,    0, DisplayName = "Minimum  > RangeStart == RangeEnd  < Maximum")]
        [DataRow(  -90,   90,             0,   90, DisplayName = "Minimum  > RangeStart  < RangeEnd  < Maximum")]
        [DataRow(  -90,  -10,             0,    0, DisplayName = "Minimum  > RangeStart  < RangeEnd  < Maximum, RangeEnd < Minimum")]
        [DataRow(  -10,  -90,             0,    0, DisplayName = "Minimum  > RangeStart  > RangeEnd  < Maximum")]
        [DataRow(   10,  -90,             0,    0, DisplayName = "Minimum  < RangeStart  > RangeEnd  < Maximum, RangeEnd < Minimum")]

        // If
        // Minimum < Maximum
        // RangeEnd >= Maximum
        //
        // Then
        // RangeEnd will be Maximum

        // If
        // Minimum < Maximum
        // RangeStart >= Maximum
        // RangeEnd >= Maximum
        //
        // Then
        // RangeStart will be Maximum
        // RangeEnd will be Maximum
        [DataRow( 150,  150,            100,  100, DisplayName = "Minimum  < RangeStart == RangeEnd  > Maximum")]
        [DataRow(  10,  190,             10,  100, DisplayName = "Minimum  < RangeStart  < RangeEnd  > Maximum")]
        [DataRow( 110,  190,            100,  100, DisplayName = "Minimum  < RangeStart  < RangeEnd  > Maximum, RangeStart > Maximum")]
        [DataRow( 190,  110,            100,  100, DisplayName = "Minimum  < RangeStart  > RangeEnd  > Maximum")]
        public Task Initialize_MinLtMax(double rangeStart, double rangeEnd, double expectedRangeStart, double expectedRangeEnd)
            => Initialize(1, 0, rangeStart, rangeEnd, 100, 1, 0, expectedRangeStart, expectedRangeEnd, 100);

        [TestCategory("Initialize")]
        [TestMethod]

        // If
        // Minimum >= Maximum 
        //
        // Then
        // Minimum will be Maximum
        // Maximum will be Maximum + 0.01

        // If
        // Minimum >= Maximum
        // RangeStart > Maximum
        // RangeEnd > Maximum
        //
        // Then
        // RangeStart will be Maximum + 0.01
        // Else
        // RangeStart will be Maximum

        // If
        // Minimum >= Maximum
        // RangeEnd > Maximum
        //
        // Then
        // RangeEnd will be Maximum + 0.01
        // Else
        // RangeEnd will be Maximum

        // Input:Start End    Expected:Min   Start End   Max
        [DataRow(    0,    0,              0,    0,    0, 0.01, DisplayName = "Minimum == RangeStart == RangeEnd == Maximum")]
        [DataRow(    0,   10,              0,    0, 0.01, 0.01, DisplayName = "Minimum == RangeStart  < RangeEnd  > Maximum")]
        [DataRow(    0,  -10,              0,    0,    0, 0.01, DisplayName = "Minimum == RangeStart  > RangeEnd  < Maximum")]
        [DataRow(  -10,    0,              0,    0,    0, 0.01, DisplayName = "Minimum  > RangeStart  < RangeEnd == Maximum")]
        [DataRow(   10,    0,              0,    0,    0, 0.01, DisplayName = "Minimum  < RangeStart  > RangeEnd == Maximum")] 
        [DataRow(   10,   90,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  < RangeStart  < RangeEnd  > Maximum")]
        [DataRow(   90,   10,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  < RangeStart  > RangeEnd  > Maximum")]
        [DataRow(  -90,  -10,              0,    0,    0, 0.01, DisplayName = "Minimum  > RangeStart  < RangeEnd  < Maximum")]
        [DataRow(  -10,  -90,              0,    0,    0, 0.01, DisplayName = "Minimum  > RangeStart  > RangeEnd  < Maximum")]
        [DataRow(   10,  -10,              0,    0,    0, 0.01, DisplayName = "Minimum  < RangeStart  > RangeEnd  < Maximum")]
        [DataRow(  -10,   10,              0,    0, 0.01, 0.01, DisplayName = "Minimum  > RangeStart  < RangeEnd  > Maximum")]
        public Task Initialize_MinEqMax(double rangeStart, double rangeEnd, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
            => Initialize(1, 0, rangeStart, rangeEnd, 0, 1, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

        [TestCategory("Initialize")]
        [TestMethod]
        [DataRow(    0,  100,              0,    0, 0.01, 0.01, DisplayName = "Minimum  > RangeStart  < RangeEnd  > Maximum, RangeStart == Maximum, RangeEnd == Minimum")]
        [DataRow(   10,   90,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > RangeStart  < RangeEnd  > Maximum")]
        [DataRow(   50,   50,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > RangeStart == RangeEnd  > Maximum")]
        [DataRow(    0,    0,              0,    0,    0, 0.01, DisplayName = "Minimum  > RangeStart == RangeEnd == Maximum")]
        [DataRow(  100,  100,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum == RangeStart == RangeEnd  > Maximum")]

        [DataRow(   10,  100,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > RangeStart  < RangeEnd  > Maximum, RangeEnd   == Maximum")]

        [DataRow(   90,   10,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > RangeStart  > RangeEnd  > Maximum")]
        [DataRow(  100,   10,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > RangeStart  > RangeEnd  > Maximum, RangeStart == Maximum")]
        [DataRow(   90,    0,              0,    0,    0, 0.01, DisplayName = "Minimum  > RangeStart  > RangeEnd  > Maximum, RangeEnd   == Maximum")]

        [DataRow(  100,    0,              0,    0,    0, 0.01, DisplayName = "Minimum == RangeStart  > RangeEnd == Maximum")]

        [DataRow(  -90,  -10,              0,    0,    0, 0.01, DisplayName = "Minimum  > RangeStart  < RangeEnd  < Maximum")]
        [DataRow(  -10,  -90,              0,    0,    0, 0.01, DisplayName = "Minimum  > RangeStart  > RangeEnd  < Maximum")]
        [DataRow(  -50,  -50,              0,    0,    0, 0.01, DisplayName = "Minimum  > RangeStart == RangeEnd  < Maximum")]

        [DataRow(  110,  190,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  < RangeStart  < RangeEnd  > Maximum")]
        [DataRow(  190,  110,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  < RangeStart  > RangeEnd  > Maximum")]
        [DataRow(  150,  150,              0, 0.01, 0.01, 0.01, DisplayName = "Minimum  < RangeStart == RangeEnd  > Maximum")]
        public Task Initialize_MinGtMax(double rangeStart, double rangeEnd, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
            => Initialize(1, 100, rangeStart, rangeEnd, 0, 1, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

        public async Task Initialize(double stepFrequency, double minimum, double rangeStart, double rangeEnd, double maximum, double expectedStepFrequency, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
        {
            var input = new TestRecord(stepFrequency, minimum, rangeStart, rangeEnd, maximum);
            var expected = new TestRecord(expectedStepFrequency, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var r = BuildRangeSelecor(input);

                await SetTestContentAsync(r);

                var actual = BuildTestRecord(r);

                Assert.AreEqual(expected, actual);
            });
        }

        [TestMethod]
        [TestCategory("Set Prop")]

        // Set:Min      Then:Min   Start   End     Max
        [DataRow(     0,         0,  10   ,  90   , 100   )]
        [DataRow(   -10,       -10,  10   ,  90   , 100   )]
        [DataRow(    10,        10,  10   ,  90   , 100   )]
        [DataRow(    50,        50,  50   ,  90   , 100   )]
        [DataRow(    90,        90,  90   ,  90   , 100   )]
        [DataRow(   100,       100, 100   , 100   , 100   )]
        [DataRow(   110,       110, 110.01, 110.01, 110.01)]
        public Task SetMinimum(double setMinimumValue, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
            => SetProp(1, 0, 10, 90, 100, Property.Minimum, setMinimumValue, 1, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

        [TestMethod]
        [TestCategory("Set Prop")]

        // Set:Max      Then:Min      Start End  Max
        [DataRow(   100,         0   ,   10,  90, 100)]
        [DataRow(   110,         0   ,   10,  90, 110)]
        [DataRow(    90,         0   ,   10,  90,  90)]
        [DataRow(    50,         0   ,   10,  50,  50)]
        [DataRow(    10,         0   ,   10,  10,  10)]
        [DataRow(     0,         0   ,    0,   0,   0)]
        [DataRow(   -10,       -10.01,  -10, -10, -10)]
        public Task SetMaximum(double propInput, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
            => SetProp(1, 0, 10, 90, 100, Property.Maximum, propInput, 1, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

        [TestMethod]
        [TestCategory("Set Prop")]

        // Set:Start  Then:Start End
        [DataRow( 10,         10,  90)]
        [DataRow(  5,          5,  90)]
        [DataRow(  0,          0,  90)]
        [DataRow(-10,          0,  90)]
        [DataRow( 90,         90,  90)]
        [DataRow( 95,         95,  95)]
        [DataRow(100,        100, 100)]
        [DataRow(110,        100, 100)]
        public Task SetRangeStart(double propInput, double expectedRangeStart, double expectedRangeEnd)
            => SetProp(1, 0, 10, 90, 100, Property.RangeStart, propInput, 1, 0, expectedRangeStart, expectedRangeEnd, 100);

        [TestMethod]
        [TestCategory("Set Prop")]
        // Given:Step Start End   Set:Start Then:Start End
        [DataRow(   5,   10,   90,       10,        10,  90)]
        [DataRow(   5,   10,   90,        5,         5,  90)]
        [DataRow(   5,   10,   90,        0,         0,  90)]
        [DataRow(   5,   10,   90,        1,         0,  90)]
        [DataRow(   5,   10,   90,      2.4,         0,  90)]
        [DataRow(   5,   10,   90,      2.5,         0,  90)]
        [DataRow(   5,   10,   90,      2.6,         5,  90)]
        [DataRow(   5,   10,   90,        4,         5,  90)]
        [DataRow(   5,   10,   90,        6,         5,  90)]
        [DataRow(   5,   10,   90,        9,        10,  90)]
        [DataRow(   5,   10,   90,      100,       100, 100)]
        [DataRow(   5,   10,   90,       89,        90,  90)]
        [DataRow(   5,   10,   90,       91,        90,  90)]
        [DataRow(  30,   60,   70,       80,       100, 100)]
        [DataRow(  30,   60,   70,       74,        60,  60)]
        [DataRow(  30,   60,   70,       75,        60,  60)]
        [DataRow(  30,   60,   70,       76,       100, 100)]
        [DataRow(  40,   40,   60,       20,         0,  60)]
        [DataRow(  40,   40,   60,       50,        40,  60)]
        [DataRow(  40,   40,   60,       60,       100, 100)]
        public Task SetRangeStart_StepTest(double stepFrequency, double rangeStart, double rangeEnd, double propInput, double expectedRangeStart, double expectedRangeEnd)
            => SetProp(stepFrequency, 0, rangeStart, rangeEnd, 100, Property.RangeStart, propInput, stepFrequency, 0, expectedRangeStart, expectedRangeEnd, 100);

        [TestMethod]
        [TestCategory("Set Prop")]

        // Set:End        Then:Start   End
        [DataRow(    90,          10,  90)]
        [DataRow(    95,          10,  95)]
        [DataRow(   100,          10, 100)]
        [DataRow(   110,          10, 100)]
        [DataRow(    10,          10,  10)]
        [DataRow(     5,           5,   5)]
        [DataRow(     0,           0,   0)]
        [DataRow(   -10,           0,   0)]
        public Task SetRangeEnd(double propInput,  double expectedRangeStart, double expectedRangeEnd)
            => SetProp(1, 0, 10, 90, 100, Property.RangeEnd, propInput, 1, 0, expectedRangeStart, expectedRangeEnd, 100);

        [TestMethod]
        [TestCategory("Set Prop")]
        // Given:Step Start End   Set:End Then:Start End
        [DataRow(  30,   60,   70,     50,        60,  60)]
        [DataRow(  30,   60,   70,     36,        30,  30)]
        [DataRow(  30,   60,   70,     35,        30,  30)]
        [DataRow(  30,   60,   70,     34,        30,  30)]
        [DataRow(  40,   40,   60,     80,        40, 100)]
        [DataRow(  40,   40,   60,     30,        40,  40)]
        [DataRow(  40,   40,   60,      0,         0,   0)]
        public Task SetRangeEnd_StepTest(double stepFrequency, double rangeStart, double rangeEnd, double propInput, double expectedRangeStart, double expectedRangeEnd)
            => SetProp(stepFrequency, 0, rangeStart, rangeEnd, 100, Property.RangeEnd, propInput, stepFrequency, 0, expectedRangeStart, expectedRangeEnd, 100);

#pragma warning restore SA1025, SA1008, SA1021

        public async Task SetProp(double stepFrequency, double minimum, double rangeStart, double rangeEnd, double maximum, Property targetProp, double propInput, double expectedStepFrequency, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
        {
            await App.DispatcherQueue.EnqueueAsync(async () =>
            {
                var inital = new TestRecord(stepFrequency, minimum, rangeStart, rangeEnd, maximum);
                var r = BuildRangeSelecor(inital);

                await SetTestContentAsync(r);

                Assert.AreEqual(inital, BuildTestRecord(r));

                switch (targetProp)
                {
                    case Property.Minimum:
                        r.Minimum = propInput;
                        break;
                    case Property.Maximum:
                        r.Maximum = propInput;
                        break;
                    case Property.RangeStart:
                        r.RangeStart = propInput;
                        break;
                    case Property.RangeEnd:
                        r.RangeEnd = propInput;
                        break;
                    case Property.StepFrequency:
                        r.StepFrequency = propInput;
                        break;
                    default:
                        Assert.Fail("Invalid param {0}", targetProp);
                        break;
                }
                
                var expected = new TestRecord(expectedStepFrequency, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

                Assert.AreEqual(expected, BuildTestRecord(r));
            });
        }

        public enum Property
        {
            StepFrequency,
            Minimum,
            Maximum,
            RangeStart,
            RangeEnd
        }

        public record TestRecord(double StepFrequency, double Minimum, double RangeStart, double RangeEnd, double Maximum);

        public static RangeSelector BuildRangeSelecor(TestRecord input)
            => new()
            {
                StepFrequency = input.StepFrequency,
                Minimum = input.Minimum,
                Maximum = input.Maximum,
                RangeStart = input.RangeStart,
                RangeEnd = input.RangeEnd,
            };

        public static TestRecord BuildTestRecord(RangeSelector r)
            => new(r.StepFrequency, r.Minimum, r.RangeStart, r.RangeEnd, r.Maximum);
    }
}