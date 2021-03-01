// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Foundation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Uwp;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_SizeExtensions
    {
        [TestCategory("SizeExtensions")]
        [TestMethod]
        [DataRow(0d, 0d)]
        [DataRow(3.14d, 6.55f)]
        [DataRow(double.MinValue, double.Epsilon)]
        public static void Test_SizeExtensions_ToRect_FromSize(double width, double height)
        {
            Size s = new Size(width, height);
            Rect
                a = s.ToRect(),
                b = new Rect(0, 0, s.Width, s.Height);

            Assert.AreEqual(a, b);
        }

        [TestCategory("SizeExtensions")]
        [TestMethod]
        [DataRow(0d, 0d, 0, 0)]
        [DataRow(0d, 0d, 22, 6.89d)]
        [DataRow(3.14d, 6.55f, 3838d, 3.24724928d)]
        [DataRow(double.MinValue, double.Epsilon, 22, 0.3248d)]
        public static void Test_SizeExtensions_ToRect_FromSizeAndPosition(double width, double height, int x, int y)
        {
            Size s = new Size(width, height);
            Rect
                a = s.ToRect(x, y),
                b = new Rect(x, y, s.Width, s.Height);

            Assert.AreEqual(a, b);
        }

        [TestCategory("SizeExtensions")]
        [TestMethod]
        [DataRow(0d, 0d, 0, 0)]
        [DataRow(0d, 0d, 22, 6.89d)]
        [DataRow(3.14d, 6.55f, 3838d, 3.24724928d)]
        [DataRow(double.MinValue, double.Epsilon, 22, 0.3248d)]
        public static void Test_SizeExtensions_ToRect_FromSizeAndPoint(double width, double height, int x, int y)
        {
            Point p = new Point(x, y);
            Size s = new Size(width, height);
            Rect
                a = s.ToRect(p),
                b = new Rect(p, s);

            Assert.AreEqual(a, b);
        }
    }
}