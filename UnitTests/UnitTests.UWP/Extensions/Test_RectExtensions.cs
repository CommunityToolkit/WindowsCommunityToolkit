// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.Toolkit.Uwp.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.Foundation;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_RectExtensions
    {
        [TestCategory("RectExtensions")]
        [TestMethod]
        [DataRow(0, 0, 2, 2, 0, 0, 2, 2, true)]// Full intersection.
        [DataRow(0, 0, 2, 2, 1, 1, 2, 2, true)]// Partial intersection.
        [DataRow(0, 0, 2, 2, 2, 0, 2, 2, true)]// Edge intersection.
        [DataRow(0, 0, 2, 2, 2, 2, 2, 2, true)]// Corner intersection.
        [DataRow(0, 0, 2, 2, 3, 0, 2, 2, false)]// No intersection.
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1117:Parameters should be on same line or separate lines", Justification = "Put the parameters of the same rectangle on the same line is clearer.")]
        public static void Test_RectExtensions_IntersectsWith(
            double rect1X, double rect1Y, double rect1Width, double rect1Height,
            double rect2X, double rect2Y, double rect2Width, double rect2Height,
            bool shouldIntersectsWith)
        {
            var rect1 = new Rect(rect1X, rect1Y, rect1Width, rect1Height);
            var rect2 = new Rect(rect2X, rect2Y, rect2Width, rect2Height);
            var isIntersectsWith = rect1.IntersectsWith(rect2);
            Assert.IsTrue(isIntersectsWith == shouldIntersectsWith);
        }
    }
}
