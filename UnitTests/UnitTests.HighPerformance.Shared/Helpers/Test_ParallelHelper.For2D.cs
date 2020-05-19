// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using Microsoft.Toolkit.HighPerformance.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Helpers
{
    public partial class Test_ParallelHelper
    {
        /// <summary>
        /// Gets the list of counts to test the For2D extensions for
        /// </summary>
        private static ReadOnlySpan<Size> TestFor2DSizes => new[]
        {
            new Size(0, 0),
            new Size(0, 1),
            new Size(1, 1),
            new Size(3, 3),
            new Size(1024, 1024),
            new Size(512, 2175),
            new Size(4039, 11231)
        };

        [TestCategory("ParallelHelper")]
        [TestMethod]
        public void Test_ParallelHelper_For2DWithIndices()
        {
            foreach (var size in TestFor2DSizes)
            {
                int[,] data = new int[size.Height, size.Width];

                ParallelHelper.For2D(0, size.Height, 0, size.Width, new Assigner2D(data));

                for (int i = 0; i < size.Height; i++)
                {
                    for (int j = 0; j < size.Width; j++)
                    {
                        if (data[i, j] != unchecked(i * 397 ^ j))
                        {
                            Assert.Fail($"Invalid item at position [{i},{j}], value was {data[i, j]} instead of {unchecked(i * 397 ^ j)}");
                        }
                    }
                }
            }
        }

#if NETCOREAPP3_1
        [TestCategory("ParallelHelper")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_ParallelHelper_For2DInvalidRange_FromEnd()
        {
            ParallelHelper.For2D<Assigner2D>(..^1, ..4);
        }

        [TestCategory("ParallelHelper")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_ParallelHelper_For2DInvalidRange_RangeAll()
        {
            ParallelHelper.For2D<Assigner2D>(..5, ..);
        }

        [TestCategory("ParallelHelper")]
        [TestMethod]
        public void Test_ParallelHelper_For2DWithRanges()
        {
            foreach (var size in TestFor2DSizes)
            {
                int[,] data = new int[size.Height, size.Width];

                ParallelHelper.For2D(..size.Height, ..size.Width, new Assigner2D(data));

                for (int i = 0; i < size.Height; i++)
                {
                    for (int j = 0; j < size.Width; j++)
                    {
                        if (data[i, j] != unchecked(i * 397 ^ j))
                        {
                            Assert.Fail($"Invalid item at position [{i},{j}], value was {data[i, j]} instead of {unchecked(i * 397 ^ j)}");
                        }
                    }
                }
            }
        }
#endif

        /// <summary>
        /// A type implementing <see cref="IAction"/> to initialize a 2D array
        /// </summary>
        private readonly struct Assigner2D : IAction2D
        {
            private readonly int[,] array;

            public Assigner2D(int[,] array) => this.array = array;

            /// <inheritdoc/>
            public void Invoke(int i, int j)
            {
                if (this.array[i, j] != 0)
                {
                    throw new InvalidOperationException($"Invalid target position [{i},{j}], was {this.array[i, j]} instead of 0");
                }

                this.array[i, j] = unchecked(i * 397 ^ j);
            }
        }
    }
}
