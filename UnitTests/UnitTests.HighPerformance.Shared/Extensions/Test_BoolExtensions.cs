// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_BoolExtensions
    {
        [TestCategory("BoolExtensions")]
        [TestMethod]
        public void Test_BoolExtensions_True()
        {
            Assert.AreEqual(1, true.ToInt(), nameof(Test_BoolExtensions_True));
            Assert.AreEqual(1, (DateTime.Now.Year > 0).ToInt(), nameof(Test_BoolExtensions_True));
        }

        [TestCategory("BoolExtensions")]
        [TestMethod]
        public void Test_BoolExtensions_False()
        {
            Assert.AreEqual(0, false.ToInt(), nameof(Test_BoolExtensions_False));
            Assert.AreEqual(0, (DateTime.Now.Year > 3000).ToInt(), nameof(Test_BoolExtensions_False));
        }
    }
}
