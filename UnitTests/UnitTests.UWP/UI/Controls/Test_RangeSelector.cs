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

        // Input:Sf   Min   Max   Start End        Expected:Sf   Min   Max   Start End
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