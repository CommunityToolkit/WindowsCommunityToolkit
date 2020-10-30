// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_RefOfT
    {
#if WINDOWS_UWP
        [TestCategory("RefOfT")]
        [TestMethod]
        public void Test_RefOfT_CreateRefOfT()
        {
            var model = new FieldOwner { Value = 1 };
            var reference = new Ref<int>(model, ref model.Value);

            // Create a Ref<T> wrapping a ref to a field in an object and validate that
            // the returned ref does indeed match one to that field directly.
            Assert.IsTrue(Unsafe.AreSame(ref model.Value, ref reference.Value));

            reference.Value++;

            // We increment the ref, and then verify the target field was updated correctly
            Assert.AreEqual(model.Value, 2);
        }

        /// <summary>
        /// A dummy model that owns an <see cref="int"/> field.
        /// </summary>
        private sealed class FieldOwner
        {
            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401", Justification = "Quick ref access for tests")]
            public int Value;
        }

        // Helper method that creates a Ref<T> to the first item of a T[] array, on UWP. In this
        // case (portable version) we need to use both the target object, and an interior reference.
        [Pure]
        private static Ref<T> CreateRefFromArray<T>(T[] array)
        {
            return new Ref<T>(array, ref array[0]);
        }
#else
        [TestCategory("RefOfT")]
        [TestMethod]
        public void Test_RefOfT_CreateRefOfT()
        {
            int value = 1;
            var reference = new Ref<int>(ref value);

            // Same as the UWP version above, but directly wrapping the target ref
            Assert.IsTrue(Unsafe.AreSame(ref value, ref reference.Value));

            reference.Value++;

            Assert.AreEqual(value, 2);
        }

        // Same as above, but here we can directly wrap the target ref
        [Pure]
        private static Ref<T> CreateRefFromArray<T>(T[] array)
        {
            return new Ref<T>(ref array[0]);
        }

        [TestCategory("RefOfT")]
        [TestMethod]
        public unsafe void Test_RefOfT_CreateRefOfTFromPointer_Ok()
        {
            int value = 1;
            var reference = new Ref<int>(&value);

            // Same as above, but this time we wrap a raw pointer to a local instead
            Assert.IsTrue(Unsafe.AreSame(ref value, ref reference.Value));

            reference.Value++;

            Assert.AreEqual(value, 2);
        }

        [TestCategory("RefOfT")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public unsafe void Test_RefOfT_CreateRefOfTFrompointer_Fail()
        {
            var reference = new Ref<string>((void*)0);

            // Creating a Ref<T> from a pointer when T is a managed type (eg. string) is not
            // allowed (this is consistent with APIs from the BCL such as Span<T>). So the
            // constructor above should always throw an ArgumentException.
            Assert.Fail();
        }
#endif

        [TestCategory("RefOfT")]
        [TestMethod]
        public void Test_RefOfT_DangerousGetReferenceAt()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            Ref<int> reference = CreateRefFromArray(array);

            // We created a Ref<T> pointing to the first item of the target array, so here
            // we test a number of ref accesses (both directly to the target ref, as well as
            // using the helper indexers to do pointer arithmetic) to validate their results.
            // Doing Ref<T>[i] is conceptually eqivalent to doing p[i] on a given T* pointer,
            // so here we compare these offsetting operations with refs from the target array.
            Assert.IsTrue(Unsafe.AreSame(ref array[0], ref reference.Value));
            Assert.IsTrue(Unsafe.AreSame(ref array[3], ref reference[3]));
            Assert.IsTrue(Unsafe.AreSame(ref array[3], ref reference[(IntPtr)3]));
        }
    }
}
