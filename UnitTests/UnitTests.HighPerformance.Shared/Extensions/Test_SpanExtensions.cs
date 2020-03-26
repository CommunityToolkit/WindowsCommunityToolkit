// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009", Justification = "List<T> of value tuple")]
        public void Test_SpanExtensions_Enumerate()
        {
            Span<int> data = new[] { 1, 2, 3, 4, 5, 6, 7 };

            List<(int Index, int Value)> values = new List<(int, int)>();

            foreach (var item in data.Enumerate())
            {
                values.Add((item.Index, item.Value));

                item.Value = item.Index * 10;
            }

            Assert.AreEqual(values.Count, data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], i * 10);
                Assert.AreEqual(i, values[i].Index);
                Assert.AreEqual(i + 1, values[i].Value);
            }
        }
    }
}
