// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_MemoryStream
    {
        [TestCategory("MemoryStream")]
        [TestMethod]
        public void Test_MemoryStream_Lifecycle()
        {
            Memory<byte> memory = new byte[100];

            Stream stream = memory.AsStream();

            Assert.IsTrue(stream.CanRead);
            Assert.IsTrue(stream.CanSeek);
            Assert.IsTrue(stream.CanWrite);
            Assert.AreEqual(stream.Length, memory.Length);
            Assert.AreEqual(stream.Position, 0);

            stream.Dispose();

            Assert.IsFalse(stream.CanRead);
            Assert.IsFalse(stream.CanSeek);
            Assert.IsFalse(stream.CanWrite);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.Length);
            Assert.ThrowsException<ObjectDisposedException>(() => stream.Position);
        }

        [TestCategory("MemoryStream")]
        [TestMethod]
        public void Test_MemoryStream_Seek()
        {
            Stream stream = new byte[100].AsMemory().AsStream();

            Assert.AreEqual(stream.Position, 0);

            stream.Position = 42;

            Assert.AreEqual(stream.Position, 42);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => stream.Position = -1);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => stream.Position = 120);
        }
    }
}
