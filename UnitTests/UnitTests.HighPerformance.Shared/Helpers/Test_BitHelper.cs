// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.HighPerformance.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_BitHelper
    {
        [TestCategory("BitHelper")]
        [TestMethod]
        public void Test_BitHelper_HasFlag_UInt32()
        {
            uint value = 0b10_1001110101_0110010010_1100100010;

            Assert.IsFalse(BitHelper.HasFlag(value, 0));
            Assert.IsTrue(BitHelper.HasFlag(value, 1));
            Assert.IsFalse(BitHelper.HasFlag(value, 2));
            Assert.IsTrue(BitHelper.HasFlag(value, 9));
            Assert.IsFalse(BitHelper.HasFlag(value, 10));
            Assert.IsFalse(BitHelper.HasFlag(value, 30));
            Assert.IsTrue(BitHelper.HasFlag(value, 31));
        }

        [TestCategory("BitHelper")]
        [TestMethod]
        public void Test_BitHelper_SetFlag_UInt32()
        {
            Assert.AreEqual(0b1u, BitHelper.SetFlag(0u, 0, true));
            Assert.AreEqual(4u, BitHelper.SetFlag(4u, 1, false));
            Assert.AreEqual(0b110u, BitHelper.SetFlag(2u, 2, true));
            Assert.AreEqual(unchecked((uint)int.MinValue), BitHelper.SetFlag(0u, 31, true));
        }

        [TestCategory("BitHelper")]
        [TestMethod]
        public void Test_UInt64Extensions_HasFlag()
        {
            ulong value = 0b10_1001110101_0110010010_1100100010;

            value |= 1ul << 63;

            Assert.IsFalse(BitHelper.HasFlag(value, 0));
            Assert.IsTrue(BitHelper.HasFlag(value, 1));
            Assert.IsFalse(BitHelper.HasFlag(value, 2));
            Assert.IsTrue(BitHelper.HasFlag(value, 9));
            Assert.IsFalse(BitHelper.HasFlag(value, 10));
            Assert.IsFalse(BitHelper.HasFlag(value, 30));
            Assert.IsTrue(BitHelper.HasFlag(value, 31));
            Assert.IsTrue(BitHelper.HasFlag(value, 63));
        }

        [TestCategory("BitHelper")]
        [TestMethod]
        public void Test_UInt64Extensions_SetFlag()
        {
            Assert.AreEqual(0b1ul, BitHelper.SetFlag(0u, 0, true));
            Assert.AreEqual(4ul, BitHelper.SetFlag(4u, 1, false));
            Assert.AreEqual(0b110ul, BitHelper.SetFlag(2u, 2, true));
            Assert.AreEqual(1ul << 31, BitHelper.SetFlag(0ul, 31, true));
            Assert.AreEqual(1ul << 63, BitHelper.SetFlag(0ul, 63, true));
        }
    }
}
