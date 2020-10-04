// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_MemoryExtensions
    {
        [TestCategory("MemoryExtensions")]
        [TestMethod]
        public void Test_MemoryExtensions_Cast_Empty()
        {
            Memory<byte> m1 = default;
            Memory<byte> mc1 = m1.Cast<byte, byte>();

            Assert.IsTrue(mc1.IsEmpty);

            Memory<byte> m2 = default;
            Memory<float> mc2 = m2.Cast<byte, float>();

            Assert.IsTrue(mc2.IsEmpty);

            Memory<short> m3 = default;
            Memory<Guid> mc3 = m3.Cast<short, Guid>();

            Assert.IsTrue(mc3.IsEmpty);

            Memory<byte> m4 = new byte[12].AsMemory(12);
            Memory<int> mc4 = m4.Cast<byte, int>();

            Assert.IsTrue(mc4.IsEmpty);

            Memory<byte> m5 = new byte[12].AsMemory().Slice(4).Slice(8);
            Memory<int> mc5 = m5.Cast<byte, int>();

            Assert.IsTrue(mc5.IsEmpty);
        }

        [TestCategory("MemoryExtensions")]
        [TestMethod]
        public void Test_MemoryExtensions_Cast_TooShort()
        {
            Memory<byte> m1 = new byte[3];
            Memory<int> mc1 = m1.Cast<byte, int>();

            Assert.IsTrue(mc1.IsEmpty);

            Memory<byte> m2 = new byte[13];
            Memory<float> mc2 = m2.Cast<byte, float>();

            Assert.AreEqual(mc2.Length, 3);

            Memory<byte> m3 = new byte[16].AsMemory(5);
            Memory<float> mc3 = m3.Cast<byte, float>();

            Assert.AreEqual(mc3.Length, 2);
        }

        [TestCategory("MemoryExtensions")]
        [TestMethod]
        public void Test_MemoryExtensions_FromArray_CastFromByte()
        {
            Memory<byte> memoryOfBytes = new byte[128];
            Memory<float> memoryOfFloats = memoryOfBytes.Cast<byte, float>();

            Assert.AreEqual(memoryOfFloats.Length, 128 / sizeof(float));

            Span<byte> spanOfBytes = memoryOfBytes.Span;
            Span<float> spanOfFloats = memoryOfFloats.Span;

            Assert.IsTrue(Unsafe.AreSame(
                ref spanOfBytes[0],
                ref Unsafe.As<float, byte>(ref spanOfFloats[0])));
        }

        [TestCategory("MemoryExtensions")]
        [TestMethod]
        public void Test_MemoryExtensions_FromArray_CastToByte()
        {
            Memory<float> memoryOfFloats = new float[128];
            Memory<byte> memoryOfBytes = memoryOfFloats.Cast<float, byte>();

            Assert.AreEqual(memoryOfBytes.Length, 128 * sizeof(float));

            Span<float> spanOfFloats = memoryOfFloats.Span;
            Span<byte> spanOfBytes = memoryOfBytes.Span;

            Assert.IsTrue(Unsafe.AreSame(
                ref spanOfFloats[0],
                ref Unsafe.As<byte, float>(ref spanOfBytes[0])));
        }

        [TestCategory("MemoryExtensions")]
        [TestMethod]
        public void Test_MemoryExtensions_FromArray_CastFromByteAndBack()
        {
            var data = new byte[128];
            Memory<byte> memoryOfBytes = data;
            Memory<float> memoryOfFloats = memoryOfBytes.Cast<byte, float>();
            Memory<byte> memoryBack = memoryOfFloats.Cast<float, byte>();

            Assert.AreEqual(memoryOfBytes.Length, memoryBack.Length);

            Assert.IsTrue(MemoryMarshal.TryGetArray<byte>(memoryBack, out var segment));
            Assert.AreSame(segment.Array!, data);
            Assert.AreEqual(segment.Offset, 0);
            Assert.AreEqual(segment.Count, data.Length);

            Assert.IsTrue(memoryOfBytes.Equals(memoryBack));
        }

        [TestCategory("MemoryExtensions")]
        [TestMethod]
        public void Test_MemoryExtensions_EmptyMemoryStream()
        {
            Memory<byte> memory = default;

            Stream stream = memory.AsStream();

            Assert.IsNotNull(stream);
            Assert.AreEqual(stream.Length, memory.Length);
            Assert.IsTrue(stream.CanWrite);
        }

        [TestCategory("MemoryExtensions")]
        [TestMethod]
        public void Test_MemoryExtensions_MemoryStream()
        {
            Memory<byte> memory = new byte[1024];

            Stream stream = memory.AsStream();

            Assert.IsNotNull(stream);
            Assert.AreEqual(stream.Length, memory.Length);
            Assert.IsTrue(stream.CanWrite);
        }
    }
}
