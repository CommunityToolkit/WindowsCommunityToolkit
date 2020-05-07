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
    public partial class Test_ReadOnlySpanExtensions
    {
        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_DangerousGetReference()
        {
            ReadOnlySpan<int> data = CreateRandomData<int>(12, default).AsSpan();

            ref int r0 = ref data.DangerousGetReference();
            ref int r1 = ref Unsafe.AsRef(data[0]);

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_DangerousGetReferenceAt_Zero()
        {
            ReadOnlySpan<int> data = CreateRandomData<int>(12, default).AsSpan();

            ref int r0 = ref data.DangerousGetReference();
            ref int r1 = ref data.DangerousGetReferenceAt(0);

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_DangerousGetReferenceAt_Index()
        {
            ReadOnlySpan<int> data = CreateRandomData<int>(12, default).AsSpan();

            ref int r0 = ref data.DangerousGetReferenceAt(5);
            ref int r1 = ref Unsafe.AsRef(data[5]);

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009", Justification = "List<T> of value tuple")]
        public void Test_ReadOnlySpanExtensions_Enumerate()
        {
            ReadOnlySpan<int> data = CreateRandomData<int>(12, default).AsSpan();

            List<(int Index, int Value)> values = new List<(int, int)>();

            foreach (var item in data.Enumerate())
            {
                values.Add(item);
            }

            Assert.AreEqual(values.Count, data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], values[i].Value);
                Assert.AreEqual(i, values[i].Index);
            }
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_Tokenize_Empty()
        {
            string text = string.Empty;

            var result = new List<string>();

            foreach (var token in text.AsSpan().Tokenize(','))
            {
                result.Add(new string(token.ToArray()));
            }

            var tokens = text.Split(',');

            CollectionAssert.AreEqual(result, tokens);
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_Tokenize_Csv()
        {
            string text = "name,surname,city,year,profession,age";

            var result = new List<string>();

            foreach (var token in text.AsSpan().Tokenize(','))
            {
                result.Add(new string(token.ToArray()));
            }

            var tokens = text.Split(',');

            CollectionAssert.AreEqual(result, tokens);
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_Tokenize_CsvWithMissingValues()
        {
            string text = ",name,,city,,,profession,,age,,";

            var result = new List<string>();

            foreach (var token in text.AsSpan().Tokenize(','))
            {
                result.Add(new string(token.ToArray()));
            }

            var tokens = text.Split(',');

            CollectionAssert.AreEqual(result, tokens);
        }
    }
}
