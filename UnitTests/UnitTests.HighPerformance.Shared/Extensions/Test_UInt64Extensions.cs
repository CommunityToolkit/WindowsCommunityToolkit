// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_UInt64Extensions
    {
        [TestCategory("Test_UInt64Extensions")]
        [TestMethod]
        public void Test_UInt64Extensions_HasFlag()
        {
            ulong value = 0b10_1001110101_0110010010_1100100010;

            value |= 1ul << 63;

            Assert.IsFalse(value.HasFlag(0));
            Assert.IsTrue(value.HasFlag(1));
            Assert.IsFalse(value.HasFlag(2));
            Assert.IsTrue(value.HasFlag(9));
            Assert.IsFalse(value.HasFlag(10));
            Assert.IsFalse(value.HasFlag(30));
            Assert.IsTrue(value.HasFlag(31));
            Assert.IsTrue(value.HasFlag(63));
        }

        [TestCategory("Test_UInt64Extensions")]
        [TestMethod]
        public void Test_UInt64Extensions_SetFlag()
        {
            Assert.AreEqual(0b1ul, 0u.SetFlag(0, true));
            Assert.AreEqual(4ul, 4u.SetFlag(1, false));
            Assert.AreEqual(0b110ul, 2u.SetFlag(2, true));
            Assert.AreEqual(unchecked((ulong)int.MinValue), 0ul.SetFlag(31, true));
            Assert.AreEqual(1ul << 63, 0ul.SetFlag(63, true));
        }
    }
}
