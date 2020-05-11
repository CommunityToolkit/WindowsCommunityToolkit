// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
        public void Test_ReadOnlySpanExtensions_IndexOf_Empty()
        {
            static void Test<T>()
            {
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    T a = default;

                    default(ReadOnlySpan<T>).IndexOf(in a);
                });

                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    ReadOnlySpan<T> data = new T[] { default };

                    data.Slice(1).IndexOf(in data[0]);
                });
            }

            Test<byte>();
            Test<int>();
            Test<Guid>();
            Test<string>();
            Test<object>();
            Test<IEnumerable<int>>();
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_IndexOf_NotEmpty()
        {
            static void Test<T>()
            {
                ReadOnlySpan<T> data = new T[] { default, default, default, default };

                for (int i = 0; i < data.Length; i++)
                {
                    Assert.AreEqual(i, data.IndexOf(in data[i]));
                }
            }

            Test<byte>();
            Test<int>();
            Test<Guid>();
            Test<string>();
            Test<object>();
            Test<IEnumerable<int>>();
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_IndexOf_NotEmpty_OutOfRange()
        {
            static void Test<T>()
            {
                // Before start
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    ReadOnlySpan<T> data = new T[] { default, default, default, default };

                    data.Slice(1).IndexOf(in data[0]);
                });

                // After end
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    ReadOnlySpan<T> data = new T[] { default, default, default, default };

                    data.Slice(0, 2).IndexOf(in data[2]);
                });

                // Local variable
                Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                {
                    var dummy = new T[] { default };
                    ReadOnlySpan<T> data = new T[] { default, default, default, default };

                    data.IndexOf(in dummy[0]);
                });
            }

            Test<byte>();
            Test<int>();
            Test<Guid>();
            Test<string>();
            Test<object>();
            Test<IEnumerable<int>>();
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_Enumerate()
        {
            ReadOnlySpan<int> data = new[] { 1, 2, 3, 4, 5, 6, 7 };

            int i = 0;

            foreach (var item in data.Enumerate())
            {
                Assert.IsTrue(Unsafe.AreSame(ref Unsafe.AsRef(data[i]), ref Unsafe.AsRef(item.Value)));
                Assert.AreEqual(i, item.Index);

                i++;
            }
        }

        [TestCategory("ReadOnlySpanExtensions")]
        [TestMethod]
        public void Test_ReadOnlySpanExtensions_Enumerate_Empty()
        {
            ReadOnlySpan<int> data = Array.Empty<int>();

            foreach (var item in data.Enumerate())
            {
                Assert.Fail("Empty source sequence");
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
