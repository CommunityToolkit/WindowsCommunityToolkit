// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCOREAPP2_1 || NETCOREAPP3_1

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_NullableRefOfT
    {
        [TestCategory("NullableRefOfT")]
        [TestMethod]
        public void Test_NullableRefOfT_CreateNullableRefOfT_Ok()
        {
            int value = 1;
            var reference = new NullableRef<int>(ref value);

            // Create a NullableRef<T> to a local, then validate that the ref
            // has value (as in, the ref T is not null), and that the reference
            // does match and correctly points to the local variable above.
            Assert.IsTrue(reference.HasValue);
            Assert.IsTrue(Unsafe.AreSame(ref value, ref reference.Value));

            reference.Value++;

            Assert.AreEqual(value, 2);
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        public void Test_NullableRefOfT_CreateNullableRefOfT_Null()
        {
            // Validate that different methods of creating a nullable ref
            // are all correctly reported as not having a valid value.
            Assert.IsFalse(default(NullableRef<int>).HasValue);
            Assert.IsFalse(NullableRef<int>.Null.HasValue);

            Assert.IsFalse(default(NullableRef<string>).HasValue);
            Assert.IsFalse(NullableRef<string>.Null.HasValue);
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_NullableRefOfT_CreateNullableRefOfT_Null_Exception()
        {
            NullableRef<int> reference = default;

            // We try to access the value to trigger the null reference check within the
            // NullableRef<T> type. The type should correctly detect that a null reference
            // is wrapped, and throw an InvalidOperationException (not a NullReferenceException).
            // This is consistent with how Nullable<T> works in the BCL as well (the type that
            // is used to represent nullable value types).
            _ = reference.Value;
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        public void Test_NullableRefOfT_CreateNullableRefOfT_ImplicitRefCast()
        {
            int value = 42;
            var reference = new Ref<int>(ref value);
            NullableRef<int> nullableRef = reference;

            // Verify that the inplicit Ref<T> ==> NullableRef<T> conversion works properly.
            // The value should be available, and the internal ref should remain the same.
            Assert.IsTrue(nullableRef.HasValue);
            Assert.IsTrue(Unsafe.AreSame(ref reference.Value, ref nullableRef.Value));
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        public void Test_NullableRefOfT_CreateNullableRefOfT_ExplicitCastOfT()
        {
            int value = 42;
            var reference = new NullableRef<int>(ref value);

            // Test the value equality by using the implicit T operator for NullableRef<T>.
            // As in, just like with normal refs, NullableRef<T> has automatic dereferencing,
            // and when cast to T it will dereference the internal ref and return th T value.
            Assert.AreEqual(value, (int)reference);
        }

        [TestCategory("NullableRefOfT")]
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Test_NullableRefOfT_CreateNullableRefOfT_ExplicitCastOfT_Exception()
        {
            NullableRef<int> invalid = default;

            // Same as above, but trying to dereference should throw an InvalidOperationException
            // since our NullableRef<T> here wraps a null reference. The value is discarded because
            // here we just need to trigger the implicit operator to verify the exception is thrown.
            _ = (int)invalid;
        }
    }
}

#endif