// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCOREAPP3_0

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_NullableByReferenceOfT
    {
        [TestCategory("NullableByReferenceOfT")]
        [TestMethod]
        public void Test_ByReferenceOfT_CreateNullableByReferenceOfT_Ok()
        {
            int value = 1;
            var reference = new NullableByReference<int>(ref value);

            Assert.IsTrue(reference.HasValue);
            Assert.IsTrue(Unsafe.AreSame(ref value, ref reference.Value));

            reference.Value++;

            Assert.AreEqual(value, 2);
        }

        [TestCategory("NullableByReferenceOfT")]
        [TestMethod]
        public void Test_ByReferenceOfT_CreateNullableByReferenceOfT_Null()
        {
            NullableByReference<int> reference = default;

            Assert.IsFalse(reference.HasValue);
        }

        [TestCategory("NullableByReferenceOfT")]
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Test_ByReferenceOfT_CreateNullableByReferenceOfT_Null_Exception()
        {
            NullableByReference<int> reference = default;

            _ = reference.Value;
        }
    }
}

#endif