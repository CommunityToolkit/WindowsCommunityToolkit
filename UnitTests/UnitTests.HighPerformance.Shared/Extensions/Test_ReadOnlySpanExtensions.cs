// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
        [DataRow(0)]
        [DataRow(4)]
        [DataRow(22)]
        [DataRow(43)]
        [DataRow(44)]
        [DataRow(45)]
        [DataRow(46)]
        [DataRow(100)]
        [DataRow(int.MaxValue)]
        [DataRow(-1)]
        [DataRow(int.MinValue)]
        public void Test_ReadOnlySpanExtensions_DangerousGetLookupReferenceAt(int i)
        {
            ReadOnlySpan<byte> table = new byte[]
            {
                0xFF, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 1, 1, 0, 1, 1, 1, 1,
                0, 1, 0, 1, 0, 1, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 1, 0, 1
            };

            ref byte ri = ref Unsafe.AsRef(table.DangerousGetLookupReferenceAt(i));

            bool isInRange = (uint)i < (uint)table.Length;

            if (isInRange)
            {
                Assert.IsTrue(Unsafe.AreSame(ref ri, ref Unsafe.AsRef(table[i])));
            }
            else
            {
                Assert.IsTrue(Unsafe.AreSame(ref ri, ref MemoryMarshal.GetReference(table)));
            }
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
        public void Test_ReadOnlySpanExtensions_Tokenize_Comma()
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
        public void Test_ReadOnlySpanExtensions_Tokenize_CommaWithMissingValues()
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
