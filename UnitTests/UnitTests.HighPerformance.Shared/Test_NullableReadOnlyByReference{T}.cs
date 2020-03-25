// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCOREAPP3_0

using System;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance
{
    [TestClass]
    public class Test_NullableReadOnlyByReferenceOfT
    {
        [TestCategory("NullableReadOnlyByReferenceOfT")]
        [TestMethod]
        public void Test_NullableReadOnlyByReferenceOfT_CreateNullableReadOnlyByReferenceOfT_Ok()
        {
            int value = 1;
            var reference = new NullableReadOnlyByReference<int>(value);

            Assert.IsTrue(reference.HasValue);
            Assert.IsTrue(Unsafe.AreSame(ref value, ref Unsafe.AsRef(reference.Value)));
        }

        [TestCategory("NullableReadOnlyByReferenceOfT")]
        [TestMethod]
        public void Test_NullableReadOnlyByReferenceOfT_CreateNullableReadOnlyByReferenceOfT_Null()
        {
            NullableReadOnlyByReference<int> reference = default;

            Assert.IsFalse(reference.HasValue);
        }

        [TestCategory("NullableReadOnlyByReferenceOfT")]
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Test_NullableReadOnlyByReferenceOfT_CreateNullableReadOnlyByReferenceOfT_Null_Exception()
        {
            NullableReadOnlyByReference<int> reference = default;

            _ = reference.Value;
        }
    }
}

#endif