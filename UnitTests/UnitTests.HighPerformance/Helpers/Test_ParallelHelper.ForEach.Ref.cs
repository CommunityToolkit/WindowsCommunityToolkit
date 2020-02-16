// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.HighPerformance.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Helpers
{
    public partial class Test_ParallelHelper
    {
        [TestCategory("ParallelHelper")]
        [TestMethod]
        public void Test_ParallelHelper_ForEach_Ref()
        {
            foreach (int count in TestForCounts)
            {
                int[] data = CreateRandomData(count);
                int[] copy = data.AsSpan().ToArray();

                foreach (ref int n in copy.AsSpan())
                {
                    n = unchecked(n * 397);
                }

                ParallelHelper.ForEach(data.AsMemory(), new Multiplier(397));

                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != copy[i])
                    {
                        Assert.Fail($"Item #{i} was not a match, was {data[i]} instead of {copy[i]}");
                    }
                }
            }
        }

        /// <summary>
        /// A type implementing <see cref="IRefAction{T}"/> to multiply array elements.
        /// </summary>
        private readonly struct Multiplier : IRefAction<int>
        {
            private readonly int factor;

            public Multiplier(int factor) => this.factor = factor;

            /// <inheritdoc/>
            public void Invoke(ref int i) => i = unchecked(i * this.factor);
        }
    }
}
