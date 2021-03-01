// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Uwp;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_PointExtensions
    {
        [TestCategory("PointExtensions")]
        [TestMethod]
        [DataRow(0d, 0d, 0, 0)]
        [DataRow(0d, 0d, 22, 6.89d)]
        [DataRow(3.14d, 6.55f, 3838d, 3.24724928d)]
        [DataRow(double.MinValue, double.Epsilon, 22, 0.3248d)]
        public static void Test_PointExtensions_ToRect_FromWidthHeight(double width, double height, int x, int y)
        {
            Point p = new Point(x, y);
            Rect
                a = p.ToRect(width, height),
                b = new Rect(x, y, width, height);

            Assert.AreEqual(a, b);
        }

        [TestCategory("SizeExtensions")]
        [TestMethod]
        [DataRow(0d, 0d, 0, 0)]
        [DataRow(0d, 0d, 22, 6.89d)]
        [DataRow(3.14d, 6.55f, 3838d, 3.24724928d)]
        [DataRow(double.MinValue, double.Epsilon, 22, 0.3248d)]
        public static void Test_PointExtensions_ToRect_FromPoint(double width, double height, int x, int y)
        {
            Point
                p1 = new Point(x, y),
                p2 = new Point(x + width, y + height);
            Rect
                a = p1.ToRect(p2),
                b = new Rect(p1, p2);

            Assert.AreEqual(a, b);
        }

        [TestCategory("SizeExtensions")]
        [TestMethod]
        [DataRow(0d, 0d, 0, 0)]
        [DataRow(0d, 0d, 22, 6.89d)]
        [DataRow(3.14d, 6.55f, 3838d, 3.24724928d)]
        [DataRow(double.MinValue, double.Epsilon, 22, 0.3248d)]
        public static void Test_PointExtensions_ToRect_FromSize(double width, double height, int x, int y)
        {
            Point p = new Point(x, y);
            Size s = new Size(width, height);
            Rect
                a = p.ToRect(s),
                b = new Rect(p, s);

            Assert.AreEqual(a, b);
        }
    }
}