// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_SpanExtensions
    {
        [TestCategory("SpanExtensions")]
        [TestMethod]
        public void Test_SpanExtensions_DangerousGetReference()
        {
            Span<int> data = new[] { 1, 2, 3, 4, 5, 6, 7 };

            ref int r0 = ref Unsafe.AsRef(data.DangerousGetReference());
            ref int r1 = ref Unsafe.AsRef(data[0]);

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("SpanExtensions")]
        [TestMethod]
        public void Test_SpanExtensions_DangerousGetReferenceAt_Zero()
        {
            Span<int> data = new[] { 1, 2, 3, 4, 5, 6, 7 };

            ref int r0 = ref Unsafe.AsRef(data.DangerousGetReference());
            ref int r1 = ref Unsafe.AsRef(data.DangerousGetReferenceAt(0));

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("SpanExtensions")]
        [TestMethod]
        public void Test_SpanExtensions_DangerousGetReferenceAt_Index()
        {
            Span<int> data = new[] { 1, 2, 3, 4, 5, 6, 7 };

            ref int r0 = ref Unsafe.AsRef(data.DangerousGetReferenceAt(5));
            ref int r1 = ref Unsafe.AsRef(data[5]);

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("SpanExtensions")]
        [TestMethod]
        public void Test_SpanExtensions_IndexOf_Empty()
        {
            static void Test<T>()
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    T a = default;

                    default(Span<T>).IndexOf(ref a);
                });

                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    Span<T> data = new T[] { default };

                    data.Slice(1).IndexOf(ref data[0]);
                });
            }

            Test<byte>();
            Test<int>();
            Test<Guid>();
            Test<string>();
            Test<object>();
            Test<IEnumerable<int>>();
        }

        [TestCategory("SpanExtensions")]
        [TestMethod]
        public void Test_SpanExtensions_IndexOf_NotEmpty()
        {
            static void Test<T>()
            {
                Span<T> data = new T[] { default, default, default, default };

                for (int i = 0; i < data.Length; i++)
                {
                    Assert.AreEqual(i, data.IndexOf(ref data[i]));
                }
            }

            Test<byte>();
            Test<int>();
            Test<Guid>();
            Test<string>();
            Test<object>();
            Test<IEnumerable<int>>();
        }

        [TestCategory("SpanExtensions")]
        [TestMethod]
        public void Test_SpanExtensions_IndexOf_NotEmpty_OutOfRange()
        {
            static void Test<T>()
            {
                // Before start
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    Span<T> data = new T[] { default, default, default, default };

                    data.Slice(1).IndexOf(ref data[0]);
                });

                // After end
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    Span<T> data = new T[] { default, default, default, default };

                    data.Slice(0, 2).IndexOf(ref data[2]);
                });

                // Local variable
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    var dummy = new T[] { default };
                    Span<T> data = new T[] { default, default, default, default };

                    data.IndexOf(ref dummy[0]);
                });
            }

            Test<byte>();
            Test<int>();
            Test<Guid>();
            Test<string>();
            Test<object>();
            Test<IEnumerable<int>>();
        }

        [TestCategory("SpanExtensions")]
        [TestMethod]
        public void Test_SpanExtensions_Enumerate()
        {
            Span<int> data = new[] { 1, 2, 3, 4, 5, 6, 7 };

            int i = 0;

            foreach (var item in data.Enumerate())
            {
                Assert.IsTrue(Unsafe.AreSame(ref data[i], ref item.Value));
                Assert.AreEqual(i, item.Index);

                i++;
            }
        }

        [TestCategory("SpanExtensions")]
        [TestMethod]
        public void Test_SpanExtensions_Enumerate_Empty()
        {
            Span<int> data = Array.Empty<int>();

            foreach (var item in data.Enumerate())
            {
                Assert.Fail("Empty source sequence");
            }
        }
    }
}
