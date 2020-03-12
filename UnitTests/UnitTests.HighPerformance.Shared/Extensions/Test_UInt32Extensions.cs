// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_UInt32Extensions
    {
        [TestCategory("UInt32Extensions")]
        [TestMethod]
        public void Test_BoolExtensions_HasFlag()
        {
            uint value = 0b10_1001110101_0110010010_1100100010;

            Assert.IsFalse(value.HasFlag(0));
            Assert.IsTrue(value.HasFlag(1));
            Assert.IsFalse(value.HasFlag(2));
            Assert.IsTrue(value.HasFlag(9));
            Assert.IsFalse(value.HasFlag(10));
            Assert.IsFalse(value.HasFlag(30));
            Assert.IsTrue(value.HasFlag(31));
        }

        [TestCategory("UInt32Extensions")]
        [TestMethod]
        public void Test_BoolExtensions_SetFlag()
        {
            Assert.AreEqual(0b1u, 0u.SetFlag(0, true));
            Assert.AreEqual(4u, 4u.SetFlag(1, false));
            Assert.AreEqual(0b110u, 2u.SetFlag(2, true));
            Assert.AreEqual(unchecked((uint)int.MinValue), 0u.SetFlag(31, true));
        }
    }
}
