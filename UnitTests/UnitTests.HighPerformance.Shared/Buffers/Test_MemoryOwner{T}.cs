// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Toolkit.HighPerformance.Buffers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.HighPerformance.Shared.Buffers;

namespace UnitTests.HighPerformance.Buffers
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_MemoryOwnerOfT
    {
        [TestCategory("MemoryOwnerOfT")]
        [TestMethod]
        public void Test_MemoryOwnerOfT_AllocateAndGetMemoryAndSpan()
        {
            using var buffer = MemoryOwner<int>.Allocate(127);

            Assert.IsTrue(buffer.Length == 127);
            Assert.IsTrue(buffer.Memory.Length == 127);
            Assert.IsTrue(buffer.Span.Length == 127);

            buffer.Span.Fill(42);

            Assert.IsTrue(buffer.Memory.Span.ToArray().All(i => i == 42));
            Assert.IsTrue(buffer.Span.ToArray().All(i => i == 42));
        }

        [TestCategory("MemoryOwnerOfT")]
        [TestMethod]
        public void Test_MemoryOwnerOfT_AllocateFromCustomPoolAndGetMemoryAndSpan()
        {
            var pool = new TrackingArrayPool<int>();

            using (var buffer = MemoryOwner<int>.Allocate(127, pool))
            {
                Assert.AreEqual(pool.RentedArrays.Count, 1);

                Assert.IsTrue(buffer.Length == 127);
                Assert.IsTrue(buffer.Memory.Length == 127);
                Assert.IsTrue(buffer.Span.Length == 127);

                buffer.Span.Fill(42);

                Assert.IsTrue(buffer.Memory.Span.ToArray().All(i => i == 42));
                Assert.IsTrue(buffer.Span.ToArray().All(i => i == 42));
            }

            Assert.AreEqual(pool.RentedArrays.Count, 0);
        }

        [TestCategory("MemoryOwnerOfT")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_MemoryOwnerOfT_InvalidRequestedSize()
        {
            using var buffer = MemoryOwner<int>.Allocate(-1);

            Assert.Fail("You shouldn't be here");
        }

        [TestCategory("MemoryOwnerOfT")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Test_MemoryOwnerOfT_DisposedMemory()
        {
            var buffer = MemoryOwner<int>.Allocate(127);

            buffer.Dispose();

            _ = buffer.Memory;
        }

        [TestCategory("MemoryOwnerOfT")]
        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void Test_MemoryOwnerOfT_DisposedSpan()
        {
            var buffer = MemoryOwner<int>.Allocate(127);

            buffer.Dispose();

            _ = buffer.Span;
        }

        [TestCategory("MemoryOwnerOfT")]
        [TestMethod]
        public void Test_MemoryOwnerOfT_MultipleDispose()
        {
            var buffer = MemoryOwner<int>.Allocate(127);

            buffer.Dispose();
            buffer.Dispose();
            buffer.Dispose();
            buffer.Dispose();

            // This test consists in just getting here without crashes.
            // We're validating that calling Dispose multiple times
            // by accident doesn't cause issues, and just does nothing.
        }

        [TestCategory("HashCodeOfT")]
        [TestMethod]
        public void Test_MemoryOwnerOfT_PooledBuffersAndClear()
        {
            using (var buffer = MemoryOwner<int>.Allocate(127))
            {
                buffer.Span.Fill(42);
            }

            using (var buffer = MemoryOwner<int>.Allocate(127))
            {
                Assert.IsTrue(buffer.Span.ToArray().All(i => i == 42));
            }

            using (var buffer = MemoryOwner<int>.Allocate(127, AllocationMode.Clear))
            {
                Assert.IsTrue(buffer.Span.ToArray().All(i => i == 0));
            }
        }
    }
}
