// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;

namespace UnitTests.UI.Controls
{
    [TestClass]
    #pragma warning disable SA1008 // Opening parenthesis must be spaced correctly
    public class Test_UniformGrid_FreeSpots
    {
        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_Basic()
        {
            var grid = new UniformGrid();

            var testref = new TakenSpotsReferenceHolder(new bool[4, 5]
                {
                    { false,  true, false,  true, false },
                    { false,  true,  true,  true, false },
                    { false,  true, false,  true, false },
                    { false, false,  true, false, false },
                });

            var results = UniformGrid.GetFreeSpot(testref, 0, false).ToArray();

            var expected = new(int row, int column)[]
                {
                    (0, 0),       (0, 2),       (0, 4),
                    (1, 0),                     (1, 4),
                    (2, 0),       (2, 2),       (2, 4),
                    (3, 0),(3, 1),       (3, 3),(3, 4)
                };

            CollectionAssert.AreEqual(
                expected,
                results, 
                "GetFreeSpot failed.  Expected:\n{0}.\nActual:\n{1}", 
                expected.ToArrayString(), 
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_FirstColumn()
        {
            var grid = new UniformGrid();

            var testref = new TakenSpotsReferenceHolder(new bool[4, 5]
                {
                    { true,  false, false,  true, false },
                    { false,  true,  true,  true, false },
                    { false,  true,  true,  true, false },
                    { false, false,  true, false, false },
                });

            var results = UniformGrid.GetFreeSpot(testref, 2, false).ToArray();

            var expected = new(int row, int column)[]
                {
                                  (0, 2),       (0, 4),
                    (1, 0),                     (1, 4),
                    (2, 0),                     (2, 4),
                    (3, 0),(3, 1),       (3, 3),(3, 4)
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed FirstColumn.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_FirstColumnEndBoundMinusOne()
        {
            var grid = new UniformGrid();

            var testref = new TakenSpotsReferenceHolder(new bool[3, 3]
                {
                    { false, false, false },
                    { false, false, false },
                    { false, false, false },
                });

            var results = UniformGrid.GetFreeSpot(testref, 2, false).ToArray();

            var expected = new(int row, int column)[]
                {
                                  (0, 2),
                    (1, 0),(1, 1),(1, 2),
                    (2, 0),(2, 1),(2, 2),
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed FirstColumn.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_FirstColumnEndBound()
        {
            var grid = new UniformGrid();

            var testref = new TakenSpotsReferenceHolder(new bool[3, 3]
                {
                    { false, false, false },
                    { false, false, false },
                    { false, false, false },
                });

            var results = UniformGrid.GetFreeSpot(testref, 3, false).ToArray();

            var expected = new(int row, int column)[]
                {
                    (0, 0),(0, 1),(0, 2),
                    (1, 0),(1, 1),(1, 2),
                    (2, 0),(2, 1),(2, 2),
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed FirstColumn.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_FirstColumnEndBound_TopDown()
        {
            var grid = new UniformGrid();

            var testref = new TakenSpotsReferenceHolder(new bool[3, 3]
                {
                    { false, false, false },
                    { false, false, false },
                    { false, false, false },
                });

            var results = UniformGrid.GetFreeSpot(testref, 3, true).ToArray();

            var expected = new(int row, int column)[]
                {
                    (0, 0),(1, 0),(2, 0),
                    (0, 1),(1, 1),(2, 1),
                    (0, 2),(1, 2),(2, 2),
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed FirstColumn.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }

        [TestCategory("UniformGrid")]
        [UITestMethod]
        public void Test_UniformGrid_GetFreeSpots_VerticalOrientation()
        {
            var grid = new UniformGrid();

            var testref = new TakenSpotsReferenceHolder(new bool[4, 5]
                {
                    { false, false, false,  true, false },
                    { false,  true,  true, false, false },
                    { true,  false, false,  true, false },
                    { false, false,  true, false, false },
                });

            var results = UniformGrid.GetFreeSpot(testref, 0, true).ToArray();

            // top-bottom, transpose of matrix above.
            var expected = new(int row, int column)[]
                {
                    (0, 0),(1, 0),       (3, 0),
                    (0, 1),       (2, 1),(3, 1),
                    (0, 2),       (2, 2),
                           (1, 3),       (3, 3),
                    (0, 4),(1, 4),(2, 4),(3, 4)
                };

            CollectionAssert.AreEqual(
                expected,
                results,
                "GetFreeSpot failed RightToLeft.  Expected:\n{0}.\nActual:\n{1}",
                expected.ToArrayString(),
                results.ToArrayString());
        }
    }
    #pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
}