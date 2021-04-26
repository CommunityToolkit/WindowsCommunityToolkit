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
#pragma warning disable SA1008, SA1025
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
        // Input:Sf   Min   Start End   Max        Expected:Sf   Min   Start End   Max
        [DataRow(   1,    0,    0,  100,  100,                 1,    0,    0,  100,  100, DisplayName = "Minimum  < Maximum, Minimum == RangeStart  < RangeEnd == Maximum")]
        [DataRow(   1,    0,   10,   90,  100,                 1,    0,   10,   90,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart  < RangeEnd  < Maximum")]
        [DataRow(   1,    0,   50,   50,  100,                 1,    0,   50,   50,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart == RangeEnd  < Maximum")]
        [DataRow(   1,    0,    0,   90,  100,                 1,    0,    0,   90,  100, DisplayName = "Minimum  < Maximum, Minimum == RangeStart  < RangeEnd  < Maximum")]
        [DataRow(   1,    0,    0,    0,  100,                 1,    0,    0,    0,  100, DisplayName = "Minimum  < Maximum, Minimum == RangeStart == RangeEnd  < Maximum")]
        [DataRow(   1,    0,   10,  100,  100,                 1,    0,   10,  100,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart  < RangeEnd == Maximum")]
        [DataRow(   1,    0,  100,  100,  100,                 1,    0,  100,  100,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart == RangeEnd == Maximum")]

        // If
        // Minimum < Maximum
        // RangeEnd <= Maximum
        // RangeEnd >= Minimum
        // RangeStart >= Minimum
        // RangeStart >= RangeEnd
        //
        // Then
        // RangeStart will be RangeEnd
        [DataRow(   1,    0,   90,   10,  100,                 1,    0,   10,   10,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart  > RangeEnd  < Maximum")]
        [DataRow(   1,    0,  110,   10,  100,                 1,    0,   10,   10,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart  > RangeEnd  < Maximum, RangeStart > Maximum")]

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
        [DataRow(   1,    0,  -50,  -50,  100,                 1,    0,    0,    0,  100, DisplayName = "Minimum  < Maximum, Minimum  > RangeStart == RangeEnd  < Maximum")]
        [DataRow(   1,    0,  -90,   90,  100,                 1,    0,    0,   90,  100, DisplayName = "Minimum  < Maximum, Minimum  > RangeStart  < RangeEnd  < Maximum")]
        [DataRow(   1,    0,  -90,  -10,  100,                 1,    0,    0,    0,  100, DisplayName = "Minimum  < Maximum, Minimum  > RangeStart  < RangeEnd  < Maximum, RangeEnd < Minimum")]
        [DataRow(   1,    0,  -10,  -90,  100,                 1,    0,    0,    0,  100, DisplayName = "Minimum  < Maximum, Minimum  > RangeStart  > RangeEnd  < Maximum")]
        [DataRow(   1,    0,   10,  -90,  100,                 1,    0,    0,    0,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart  > RangeEnd  < Maximum, RangeEnd < Minimum")]

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
        [DataRow(   1,    0,  150,  150,  100,                 1,    0,  100,  100,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart == RangeEnd  > Maximum")]
        [DataRow(   1,    0,   10,  190,  100,                 1,    0,   10,  100,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart  < RangeEnd  > Maximum")]
        [DataRow(   1,    0,  110,  190,  100,                 1,    0,  100,  100,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart  < RangeEnd  > Maximum, RangeStart > Maximum")]
        [DataRow(   1,    0,  190,  110,  100,                 1,    0,  100,  100,  100, DisplayName = "Minimum  < Maximum, Minimum  < RangeStart  > RangeEnd  > Maximum")]

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

        // Input:Sf   Min   Start End   Max        Expected:Sf   Min   Start End   Max
        [DataRow(   1,    0,    0,    0,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum == Maximum, Minimum == RangeStart == RangeEnd == Maximum")]
        [DataRow(   1,    0,    0,   10,    0,                 1,    0,    0, 0.01, 0.01, DisplayName = "Minimum == Maximum, Minimum == RangeStart  < RangeEnd  > Maximum")]
        [DataRow(   1,    0,    0,  -10,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum == Maximum, Minimum == RangeStart  > RangeEnd  < Maximum")]
        [DataRow(   1,    0,  -10,    0,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum == Maximum, Minimum  > RangeStart  < RangeEnd == Maximum")]
        [DataRow(   1,    0,   10,    0,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum == Maximum, Minimum  < RangeStart  > RangeEnd == Maximum")] 
        [DataRow(   1,    0,   10,   90,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum == Maximum, Minimum  < RangeStart  < RangeEnd  > Maximum")]
        [DataRow(   1,    0,   90,   10,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum == Maximum, Minimum  < RangeStart  > RangeEnd  > Maximum")]
        [DataRow(   1,    0,  -90,  -10,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum == Maximum, Minimum  > RangeStart  < RangeEnd  < Maximum")]
        [DataRow(   1,    0,  -10,  -90,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum == Maximum, Minimum  > RangeStart  > RangeEnd  < Maximum")]
        [DataRow(   1,    0,   10,  -10,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum == Maximum, Minimum  < RangeStart  > RangeEnd  < Maximum")]
        [DataRow(   1,    0,  -10,   10,    0,                 1,    0,    0, 0.01, 0.01, DisplayName = "Minimum == Maximum, Minimum  > RangeStart  < RangeEnd  > Maximum")]

        [DataRow(   1,  100,    0,  100,    0,                 1,    0,    0, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart  < RangeEnd  > Maximum, RangeStart == Maximum, RangeEnd == Minimum")]
        [DataRow(   1,  100,   10,   90,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart  < RangeEnd  > Maximum")]
        [DataRow(   1,  100,   50,   50,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart == RangeEnd  > Maximum")]
        [DataRow(   1,  100,    0,    0,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart == RangeEnd == Maximum")]
        [DataRow(   1,  100,  100,  100,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum == RangeStart == RangeEnd  > Maximum")]

        [DataRow(   1,  100,   10,  100,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart  < RangeEnd  > Maximum, RangeEnd   == Maximum")]

        [DataRow(   1,  100,   90,   10,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart  > RangeEnd  > Maximum")]
        [DataRow(   1,  100,  100,   10,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart  > RangeEnd  > Maximum, RangeStart == Maximum")]
        [DataRow(   1,  100,   90,    0,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart  > RangeEnd  > Maximum, RangeEnd   == Maximum")]

        [DataRow(   1,  100,  100,    0,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum  > Maximum, Minimum == RangeStart  > RangeEnd == Maximum")]

        [DataRow(   1,  100,  -90,  -10,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart  < RangeEnd  < Maximum")]
        [DataRow(   1,  100,  -10,  -90,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart  > RangeEnd  < Maximum")]
        [DataRow(   1,  100,  -50,  -50,    0,                 1,    0,    0,    0, 0.01, DisplayName = "Minimum  > Maximum, Minimum  > RangeStart == RangeEnd  < Maximum")]

        [DataRow(   1,  100,  110,  190,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  < RangeStart  < RangeEnd  > Maximum")]
        [DataRow(   1,  100,  190,  110,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  < RangeStart  > RangeEnd  > Maximum")]
        [DataRow(   1,  100,  150,  150,    0,                 1,    0, 0.01, 0.01, 0.01, DisplayName = "Minimum  > Maximum, Minimum  < RangeStart == RangeEnd  > Maximum")]
#pragma warning restore SA1025, SA1008
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

        // Given:Sf   Min   Max   Start End      Set:Minimum       Then:Sf  Min     Max     Start   End
#pragma warning disable SA1008, SA1025
        [DataRow(   1,    0,   10,   90,  100,             0,             1,    0   ,  10   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,           -10,             1,  -10   ,  10   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            10,             1,   10   ,  10   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            50,             1,   50   ,  50   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            90,             1,   90   ,  90   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,           100,             1,  100   , 100   , 100   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,           110,             1,  110   , 110.01, 110.01, 110.01)]
        public Task SetMinimum(double stepFrequency, double minimum, double rangeStart, double rangeEnd, double maximum, double setMinimumValue, double expectedStepFrequency, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
            => SetProp(stepFrequency, minimum, rangeStart, rangeEnd, maximum, Property.Minimum, setMinimumValue, expectedStepFrequency, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

        [TestMethod]
        [TestCategory("Set Prop")]

        // Given:Sf   Min   Max   Start End      Set:Maximum       Then:Sf  Min      Max     Start   End
        [DataRow(   1,    0,   10,   90,  100,           100,             1,    0   ,  10   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,           110,             1,    0   ,  10   ,  90   , 110   )]
        [DataRow(   1,    0,   10,   90,  100,            90,             1,    0   ,  10   ,  90   ,  90   )]
        [DataRow(   1,    0,   10,   90,  100,            50,             1,    0   ,  10   ,  50   ,  50   )]
        [DataRow(   1,    0,   10,   90,  100,            10,             1,    0   ,  10   ,  10   ,  10   )]
        [DataRow(   1,    0,   10,   90,  100,             0,             1,    0   ,   0   ,   0   ,   0   )]
        [DataRow(   1,    0,   10,   90,  100,           -10,             1,  -10.01, -10   , -10   , -10   )]
        public Task SetMaximum(double stepFrequency, double minimum, double rangeStart, double rangeEnd, double maximum, double propInput, double expectedStepFrequency, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
            => SetProp(stepFrequency, minimum, rangeStart, rangeEnd, maximum, Property.Maximum, propInput, expectedStepFrequency, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

        [TestMethod]
        [TestCategory("Set Prop")]

        // Given:Sf   Min   Max   Start End    Set:RangeStart       Then:Sf  Min      Max     Start   End
        [DataRow(   1,    0,   10,   90,  100,             10,             1,    0   ,  10   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,              5,             1,    0   ,   5   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,              0,             1,    0   ,   0   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            -10,             1,    0   ,   0   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,             90,             1,    0   ,  90   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,             95,             1,    0   ,  95   ,  95   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            100,             1,    0   , 100   , 100   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            110,             1,    0   , 100   , 100   , 100   )]

        [DataRow(   5,    0,   10,   90,  100,             10,             5,    0   ,  10   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,              5,             5,    0   ,   5   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,              0,             5,    0   ,   0   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,              1,             5,    0   ,   0   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,            2.4,             5,    0   ,   0   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,            2.5,             5,    0   ,   0   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,            2.6,             5,    0   ,   5   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,              4,             5,    0   ,   5   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,              6,             5,    0   ,   5   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,              9,             5,    0   ,  10   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,            100,             5,    0   , 100   , 100   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,             89,             5,    0   ,  90   ,  90   , 100   )]
        [DataRow(   5,    0,   10,   90,  100,             91,             5,    0   ,  90   ,  90   , 100   )]

        [DataRow(  30,    0,   60,   70,  100,             80,            30,    0   , 100   , 100   , 100   )]
        [DataRow(  30,    0,   60,   70,  100,             74,            30,    0   ,  60   ,  60   , 100   )]
        [DataRow(  30,    0,   60,   70,  100,             75,            30,    0   ,  60   ,  60   , 100   )]
        [DataRow(  30,    0,   60,   70,  100,             76,            30,    0   , 100   , 100   , 100   )]
        [DataRow(  40,    0,   40,   60,  100,             20,            40,    0   ,   0   ,  60   , 100   )]
        [DataRow(  40,    0,   40,   60,  100,             50,            40,    0   ,  40   ,  60   , 100   )]
        [DataRow(  40,    0,   40,   60,  100,             60,            40,    0   , 100   , 100   , 100   )]
        public Task SetRangeStart(double stepFrequency, double minimum, double rangeStart, double rangeEnd, double maximum, double propInput, double expectedStepFrequency, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
            => SetProp(stepFrequency, minimum, rangeStart, rangeEnd, maximum, Property.RangeStart, propInput, expectedStepFrequency, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

        [TestMethod]
        [TestCategory("Set Prop")]

        // Given:Sf   Min   Max   Start End      Set:RangeEnd       Then:Sf  Min      Max     Start   End
        [DataRow(   1,    0,   10,   90,  100,             90,             1,    0   ,  10   ,  90   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,             95,             1,    0   ,  10   ,  95   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            100,             1,    0   ,  10   , 100   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            110,             1,    0   ,  10   , 100   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,             10,             1,    0   ,  10   ,  10   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,              5,             1,    0   ,   5   ,   5   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,              0,             1,    0   ,   0   ,   0   , 100   )]
        [DataRow(   1,    0,   10,   90,  100,            -10,             1,    0   ,   0   ,   0   , 100   )]

        [DataRow(  30,    0,   60,   70,  100,             50,            30,    0   ,  60   ,  60   , 100   )]
        [DataRow(  30,    0,   60,   70,  100,             36,            30,    0   ,  30   ,  30   , 100   )]
        [DataRow(  30,    0,   60,   70,  100,             35,            30,    0   ,  30   ,  30   , 100   )]
        [DataRow(  30,    0,   60,   70,  100,             34,            30,    0   ,  30   ,  30   , 100   )]
        [DataRow(  40,    0,   40,   60,  100,             80,            40,    0   ,  40   , 100   , 100   )]
        [DataRow(  40,    0,   40,   60,  100,             30,            40,    0   ,  40   ,  40   , 100   )]
        [DataRow(  40,    0,   40,   60,  100,              0,            40,    0   ,   0   ,   0   , 100   )]
#pragma warning restore SA1025, SA1008
        public Task SetRangeEnd(double stepFrequency, double minimum, double rangeStart, double rangeEnd, double maximum, double propInput, double expectedStepFrequency, double expectedMinimum, double expectedRangeStart, double expectedRangeEnd, double expectedMaximum)
            => SetProp(stepFrequency, minimum, rangeStart, rangeEnd, maximum, Property.RangeEnd, propInput, expectedStepFrequency, expectedMinimum, expectedRangeStart, expectedRangeEnd, expectedMaximum);

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