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
    /* ==================================================================
     *                                NOTE
     * ==================================================================
     * All thests here mirror the ones for Ref<T>, as the two types are
     * the same except for the fact that this wraps a readonly ref. See
     * comments in the Ref<T> test file for more info on these tests. */
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_ReadOnlyRefOfT
    {
#if WINDOWS_UWP
        [TestCategory("ReadOnlyRefOfT")]
        [TestMethod]
        public void Test_ReadOnlyRefOfT_CreateReadOnlyRefOfT()
        {
            var model = new ReadOnlyFieldOwner();
            var reference = new ReadOnlyRef<int>(model, model.Value);

            Assert.IsTrue(Unsafe.AreSame(ref Unsafe.AsRef(model.Value), ref Unsafe.AsRef(reference.Value)));
        }

        /// <summary>
        /// A dummy model that owns an <see cref="int"/> field.
        /// </summary>
        private sealed class ReadOnlyFieldOwner
        {
            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401", Justification = "Ref readonly access for tests")]
            public readonly int Value = 1;
        }

        [Pure]
        private static ReadOnlyRef<T> CreateReadOnlyRefFromArray<T>(T[] array)
        {
            return new ReadOnlyRef<T>(array, array[0]);
        }
#else
        [TestCategory("ReadOnlyRefOfT")]
        [TestMethod]
        public void Test_ReadOnlyRefOfT_CreateReadOnlyRefOfT()
        {
            int value = 1;
            var reference = new ReadOnlyRef<int>(value);

            Assert.IsTrue(Unsafe.AreSame(ref value, ref Unsafe.AsRef(reference.Value)));
        }

        [Pure]
        private static ReadOnlyRef<T> CreateReadOnlyRefFromArray<T>(T[] array)
        {
            return new ReadOnlyRef<T>(array[0]);
        }

        [TestCategory("ReadOnlyRefOfT")]
        [TestMethod]
        public unsafe void Test_ReadOnlyRefOfT_CreateReadOnlyRefOfTFromPointer_Ok()
        {
            int value = 1;
            var reference = new ReadOnlyRef<int>(&value);

            Assert.IsTrue(Unsafe.AreSame(ref value, ref Unsafe.AsRef(reference.Value)));
        }

        [TestCategory("ReadOnlyRefOfT")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public unsafe void Test_ReadOnlyRefOfT_CreateReadOnlyRefOfTFrompointer_Fail()
        {
            var reference = new ReadOnlyRef<string>((void*)0);

            Assert.Fail();
        }
#endif

        [TestCategory("ReadOnlyRefOfT")]
        [TestMethod]
        public void Test_ReadOnlyRefOfT_DangerousGetReferenceAt()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            ReadOnlyRef<int> reference = CreateReadOnlyRefFromArray(array);

            Assert.IsTrue(Unsafe.AreSame(ref array[0], ref reference.DangerousGetReference()));
            Assert.IsTrue(Unsafe.AreSame(ref array[3], ref Unsafe.AsRef(reference[3])));
            Assert.IsTrue(Unsafe.AreSame(ref array[3], ref Unsafe.AsRef(reference[(IntPtr)3])));
            Assert.IsTrue(Unsafe.AreSame(ref array[3], ref reference.DangerousGetReferenceAt(3)));
            Assert.IsTrue(Unsafe.AreSame(ref array[3], ref reference.DangerousGetReferenceAt((IntPtr)3)));
        }
    }
}
