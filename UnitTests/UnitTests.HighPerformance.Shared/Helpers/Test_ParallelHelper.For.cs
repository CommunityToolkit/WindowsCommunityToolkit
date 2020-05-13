// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.Toolkit.HighPerformance.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Helpers
{
    [TestClass]
    public partial class Test_ParallelHelper
    {
        /// <summary>
        /// Gets the list of counts to test the For (1D) extensions for
        /// </summary>
        private static ReadOnlySpan<int> TestForCounts => new[] { 0, 1, 7, 128, 255, 256, short.MaxValue, short.MaxValue + 1, 123_938, 1_678_922, 71_890_819 };

        [TestCategory("ParallelHelper")]
        [TestMethod]
        public void Test_ParallelHelper_ForWithIndices()
        {
            foreach (int count in TestForCounts)
            {
                int[] data = new int[count];

                ParallelHelper.For(0, data.Length, new Assigner(data));

                foreach (var item in data.Enumerate())
                {
                    if (item.Index != item.Value)
                    {
                        Assert.Fail($"Invalid item at position {item.Index}, value was {item.Value}");
                    }
                }
            }
        }

#if NETCOREAPP3_1
        [TestCategory("ParallelHelper")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_ParallelHelper_ForInvalidRange_FromEnd()
        {
            ParallelHelper.For<Assigner>(..^1);
        }

        [TestCategory("ParallelHelper")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_ParallelHelper_ForInvalidRange_RangeAll()
        {
            ParallelHelper.For<Assigner>(..);
        }

        [TestCategory("ParallelHelper")]
        [TestMethod]
        public void Test_ParallelHelper_ForWithRanges()
        {
            foreach (int count in TestForCounts)
            {
                int[] data = new int[count];

                ParallelHelper.For(..data.Length, new Assigner(data));

                foreach (var item in data.Enumerate())
                {
                    if (item.Index != item.Value)
                    {
                        Assert.Fail($"Invalid item at position {item.Index}, value was {item.Value}");
                    }
                }
            }
        }
#endif

        /// <summary>
        /// A type implementing <see cref="IAction"/> to initialize an array
        /// </summary>
        private readonly struct Assigner : IAction
        {
            private readonly int[] array;

            public Assigner(int[] array) => this.array = array;

            /// <inheritdoc/>
            public void Invoke(int i)
            {
                if (this.array[i] != 0)
                {
                    throw new InvalidOperationException($"Invalid target position {i}, was {this.array[i]} instead of 0");
                }

                this.array[i] = i;
            }
        }
    }
}
