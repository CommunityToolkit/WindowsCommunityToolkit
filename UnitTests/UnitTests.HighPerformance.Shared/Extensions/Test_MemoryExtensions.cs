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
    public class Test_MemoryExtensions
    {
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
