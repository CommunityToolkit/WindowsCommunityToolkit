// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_ListExtensions
    {
        [TestCategory("ListExtensions")]
        [TestMethod]
        public void Test_ListExtensions_DangerousGetReference()
        {
            var data = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            ref int r0 = ref Unsafe.AsRef(data.DangerousGetReference());

            r0 = 99;

            Assert.AreEqual(data[0], r0);
        }

        [TestCategory("ListExtensions")]
        [TestMethod]
        public void Test_ListExtensions_DangerousGetReferenceAt_Zero()
        {
            var data = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            ref int r0 = ref Unsafe.AsRef(data.DangerousGetReferenceAt(0));

            r0 = 99;

            Assert.AreEqual(data[0], r0);
        }

        [TestCategory("ListExtensions")]
        [TestMethod]
        public void Test_ListExtensions_DangerousGetReferenceAt_Index()
        {
            var data = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            ref int r0 = ref Unsafe.AsRef(data.DangerousGetReferenceAt(6));

            r0 = 99;

            Assert.AreEqual(data[6], r0);
        }

        [TestCategory("ListExtensions")]
        [TestMethod]
        public void Test_ListExtensions_Count()
        {
            var random = new Random();

            var data = Enumerable.Range(0, 1024).Select(_ => random.Next(0, byte.MaxValue)).ToList();

            int result = data.Count(127);
            int expected = data.Count(i => i == 127);

            Assert.AreEqual(result, expected);
        }
    }
}
