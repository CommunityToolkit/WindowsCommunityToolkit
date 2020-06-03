// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Helpers
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_HashCodeOfT
    {
        /// <summary>
        /// Gets the list of counts to test the extension for
        /// </summary>
        private static ReadOnlySpan<int> TestCounts => new[] { 0, 1, 7, 128, 255, 256, short.MaxValue, short.MaxValue + 1, 123_938, 1_678_922, 71_890_819 };

        [TestCategory("HashCodeOfT")]
        [TestMethod]
        public void Test_HashCodeOfT_VectorSupportedTypes_TestRepeatCount8()
        {
            TestForType<byte>();
            TestForType<sbyte>();
            TestForType<bool>();
        }

        [TestCategory("HashCodeOfT")]
        [TestMethod]
        public void Test_HashCodeOfT_VectorSupportedTypes_TestRepeatCount16()
        {
            TestForType<ushort>();
            TestForType<short>();
        }

        [TestCategory("HashCodeOfT")]
        [TestMethod]
        public void Test_HashCodeOfT_VectorSupportedTypes_TestRepeatCount32()
        {
            TestForType<uint>();
            TestForType<int>();
            TestForType<float>();
        }

        [TestCategory("HashCodeOfT")]
        [TestMethod]
        public void Test_HashCodeOfT_VectorSupportedTypes_TestRepeatCount64()
        {
            TestForType<ulong>();
            TestForType<long>();
            TestForType<double>();
        }

        [TestCategory("HashCodeOfT")]
        [TestMethod]
        public void Test_HashCodeOfT_VectorUnsupportedTypes_TestRepeat()
        {
            TestForType<char>();
        }

#if NETCOREAPP3_1
        [TestCategory("HashCodeOfT")]
        [TestMethod]
        public void Test_HashCodeOfT_ManagedType_TestRepeat()
        {
            var random = new Random();

            foreach (var count in TestCounts.Slice(0, 8))
            {
                string[] data = new string[count];

                foreach (ref string text in data.AsSpan())
                {
                    text = random.NextDouble().ToString("E");
                }

                int hash1 = HashCode<string>.Combine(data);
                int hash2 = HashCode<string>.Combine(data);

                Assert.AreEqual(hash1, hash2, $"Failed {typeof(string)} test with count {count}: got {hash1} and then {hash2}");
            }
        }
#endif

        /// <summary>
        /// Performs a test for a specified type.
        /// </summary>
        /// <typeparam name="T">The type to test.</typeparam>
        private static void TestForType<T>()
            where T : unmanaged, IEquatable<T>
        {
            foreach (var count in TestCounts)
            {
                T[] data = CreateRandomData<T>(count);

                int hash1 = HashCode<T>.Combine(data);
                int hash2 = HashCode<T>.Combine(data);

                Assert.AreEqual(hash1, hash2, $"Failed {typeof(T)} test with count {count}: got {hash1} and then {hash2}");
            }
        }

        /// <summary>
        /// Creates a random <typeparamref name="T"/> array filled with random data.
        /// </summary>
        /// <typeparam name="T">The type of items to put in the array.</typeparam>
        /// <param name="count">The number of array items to create.</param>
        /// <returns>An array of random <typeparamref name="T"/> elements.</returns>
        [Pure]
        private static T[] CreateRandomData<T>(int count)
            where T : unmanaged
        {
            var random = new Random(count);

            T[] data = new T[count];

            foreach (ref byte n in MemoryMarshal.AsBytes(data.AsSpan()))
            {
                n = (byte)random.Next(0, byte.MaxValue);
            }

            return data;
        }
    }
}
