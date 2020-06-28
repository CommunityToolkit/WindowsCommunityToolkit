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

            Assert.IsTrue(Unsafe.AreSame(ref model.Value, ref reference.Value));

            reference.Value++;

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

            Assert.IsTrue(Unsafe.AreSame(ref value, ref reference.Value));

            reference.Value++;

            Assert.AreEqual(value, 2);
        }

        [Pure]
        private static Ref<T> CreateRefFromArray<T>(T[] array)
        {
            return new Ref<T>(ref array[0]);
        }
#endif

        [TestCategory("RefOfT")]
        [TestMethod]
        public void Test_RefOfT_DangerousGetReferenceAt()
        {
            int[] array = { 1, 2, 3, 4, 5 };
            Ref<int> reference = CreateRefFromArray(array);

            Assert.IsTrue(Unsafe.AreSame(ref array[0], ref reference.Value));
            Assert.IsTrue(Unsafe.AreSame(ref array[3], ref reference.DangerousGetReferenceAt(3)));
            Assert.IsTrue(Unsafe.AreSame(ref array[3], ref reference.DangerousGetReferenceAt((IntPtr)3)));
        }
    }
}
