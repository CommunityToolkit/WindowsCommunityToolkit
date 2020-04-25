// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_ArrayExtensions
    {
        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_FillArrayMid()
        {
            bool[,] test = new bool[4, 5];

            test.Fill(true, 1, 1, 3, 2);

            var expected = new bool[,]
                {
                    { false, false, false, false, false },
                    { false,  true,  true,  true, false },
                    { false,  true,  true,  true, false },
                    { false, false, false, false, false },
                };

            CollectionAssert.AreEqual(
                expected,
                test,
                "Fill failed.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                test.ToArrayString());
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_FillArrayTwice()
        {
            bool[,] test = new bool[4, 5];

            test.Fill(true, 0, 0, 1, 2);
            test.Fill(true, 1, 3, 2, 2);

            var expected = new bool[,]
                {
                    { true,  false, false, false, false },
                    { true,  false, false,  true,  true },
                    { false, false, false,  true,  true },
                    { false, false, false, false, false },
                };

            CollectionAssert.AreEqual(
                expected,
                test,
                "Fill failed.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                test.ToArrayString());
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_FillArrayNegativeSize()
        {
            bool[,] test = new bool[4, 5];

            test.Fill(true, 3, 4, -3, -2);

            // TODO: We may want to think about this pattern in the future:
            /*var expected = new bool[,]
                {
                    { false, false, false, false, false },
                    { false, false, false, false, false },
                    { false, false,  true,  true,  true },
                    { false, false,  true,  true,  true },
                };*/

            var expected = new bool[,]
                {
                    { false, false, false, false, false },
                    { false, false, false, false, false },
                    { false, false, false, false, false },
                    { false, false, false, false, false },
                };

            CollectionAssert.AreEqual(
                expected,
                test,
                "Fill failed.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                test.ToArrayString());
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_FillArrayBottomEdgeBoundary()
        {
            bool[,] test = new bool[4, 5];

            test.Fill(true, 1, 2, 2, 4);

            var expected = new bool[,]
                {
                    { false, false, false, false, false },
                    { false, false,  true,  true, false },
                    { false, false,  true,  true, false },
                    { false, false,  true,  true, false },
                };

            CollectionAssert.AreEqual(
                expected,
                test,
                "Fill failed.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                test.ToArrayString());
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_FillArrayTopLeftCornerNegativeBoundary()
        {
            bool[,] test = new bool[4, 5];

            test.Fill(true, -1, -1, 3, 3);

            var expected = new bool[,]
                {
                    { true,   true, false, false, false },
                    { true,   true, false, false, false },
                    { false, false, false, false, false },
                    { false, false, false, false, false },
                };

            CollectionAssert.AreEqual(
                expected,
                test,
                "Fill failed.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                test.ToArrayString());
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_FillArrayBottomRightCornerBoundary()
        {
            bool[,] test = new bool[5, 4];

            test.Fill(true, 3, 2, 3, 3);

            var expected = new bool[,]
                {
                    { false, false, false, false },
                    { false, false, false, false },
                    { false, false, false, false },
                    { false, false,  true,  true },
                    { false, false,  true,  true },
                };

            CollectionAssert.AreEqual(
                expected,
                test,
                "Fill failed.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                test.ToArrayString());
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_Jagged_GetColumn()
        {
            int[][] array =
            {
                new int[] { 5, 2, 4 }, 
                new int[] { 6, 3 }, 
                new int[] { 7 } 
            };

            var col = array.GetColumn(1).ToArray();

            CollectionAssert.AreEquivalent(new int[] { 2, 3, 0 }, col);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_Jagged_GetColumn_Exception()
        {
            int[][] array = 
            {
                new int[] { 5, 2, 4 },
                new int[] { 6, 3 },
                new int[] { 7 }
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array.GetColumn(-1).ToArray();
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array.GetColumn(3).ToArray();
            });
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_Rectangular_GetColumn()
        {
            int[,] array =
            {
                { 5,  2, 4 },
                { 6,  3, 9 },
                { 7, -1, 0 }
            };

            var col = array.GetColumn(1).ToArray();

            CollectionAssert.AreEquivalent(new int[] { 2, 3, -1 }, col);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_Rectangular_GetColumn_Exception()
        {
            int[,] array =
            {
                { 5, 2, 4 },
                { 6, 3, 0 },
                { 7, 0, 0 }
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array.GetColumn(-1).ToArray();
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array.GetColumn(3).ToArray();
            });
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_Rectangular_GetRow()
        {
            int[,] array =
            {
                { 5,  2, 4 },
                { 6,  3, 9 },
                { 7, -1, 0 }
            };

            var col = array.GetRow(1).ToArray();

            CollectionAssert.AreEquivalent(new int[] { 6, 3, 9 }, col);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_Rectangular_GetRow_Exception()
        {
            int[,] array =
            {
                { 5, 2, 4 },
                { 6, 3, 0 },
                { 7, 0, 0 }
            };

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array.GetRow(-1).ToArray();
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            {
                array.GetRow(3).ToArray();
            });
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_Rectangular_ToString()
        {
            int[,] array =
            {
                { 5, 2,  4 },
                { 6, 3, -1 },
                { 7, 0,  9 }
            };

            string value = array.ToArrayString();

            Debug.WriteLine(value);

            Assert.AreEqual("[[5,\t2,\t4]," + Environment.NewLine + " [6,\t3,\t-1]," + Environment.NewLine + " [7,\t0,\t9]]", value);
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_Jagged_ToString()
        {
            int[][] array =
            {
                new int[] { 5, 2 },
                new int[] { 6, 3, -1, 2 },
                new int[] { 7, 0,  9 }
            };

            string value = array.ToArrayString();

            Debug.WriteLine(value);

            Assert.AreEqual("[[5,\t2]," + Environment.NewLine + " [6,\t3,\t-1,\t2]," + Environment.NewLine + " [7,\t0,\t9]]", value);
        }
    }
}
