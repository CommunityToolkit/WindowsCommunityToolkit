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
    public class Test_NullableReadOnlyRefOfT
    {
        [TestCategory("NullableReadOnlyRefOfT")]
        [TestMethod]
        public void Test_NullableReadOnlyRefOfT_CreateNullableReadOnlyRefOfT_Ok()
        {
            int value = 1;
            var reference = new NullableReadOnlyRef<int>(value);

            Assert.IsTrue(reference.HasValue);
            Assert.IsTrue(Unsafe.AreSame(ref value, ref Unsafe.AsRef(reference.Value)));
        }

        [TestCategory("NullableReadOnlyRefOfT")]
        [TestMethod]
        public void Test_NullableReadOnlyRefOfT_CreateNullableReadOnlyRefOfT_Null()
        {
            NullableReadOnlyRef<int> reference = default;

            Assert.IsFalse(reference.HasValue);
        }

        [TestCategory("NullableReadOnlyRefOfT")]
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Test_NullableReadOnlyRefOfT_CreateNullableReadOnlyRefOfT_Null_Exception()
        {
            NullableReadOnlyRef<int> reference = default;

            _ = reference.Value;
        }
    }
}

#endif