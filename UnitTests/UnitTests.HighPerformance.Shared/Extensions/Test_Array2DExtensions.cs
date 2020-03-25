// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCOREAPP3_0
using System;
#endif
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public class Test_Array2DExtensions
    {
        [TestCategory("Array2DExtensions")]
        [TestMethod]
        public void Test_Array2DExtensions_DangerousGetReference_Int()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            ref int r0 = ref array.DangerousGetReference();
            ref int r1 = ref array[0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("Array2DExtensions")]
        [TestMethod]
        public void Test_Array2DExtensions_DangerousGetReference_String()
        {
            string[,] array =
            {
                { "a", "bb", "ccc" },
                { "dddd", "eeeee", "ffffff" }
            };

            ref string r0 = ref array.DangerousGetReference();
            ref string r1 = ref array[0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("Array2DExtensions")]
        [TestMethod]
        public void Test_Array2DExtensions_DangerousGetReferenceAt_Zero()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            ref int r0 = ref array.DangerousGetReferenceAt(0, 0);
            ref int r1 = ref array[0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("Array2DExtensions")]
        [TestMethod]
        public void Test_Array2DExtensions_DangerousGetReferenceAt_Index()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            ref int r0 = ref array.DangerousGetReferenceAt(1, 3);
            ref int r1 = ref array[1, 3];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

#if NETCOREAPP3_0
        [TestCategory("Array2DExtensions")]
        [TestMethod]
        public void Test_Array2DExtensions_AsSpan_Empty()
        {
            int[,] array = new int[0, 0];

            Span<int> span = array.AsSpan();

            Assert.AreEqual(span.Length, array.Length);
            Assert.IsTrue(span.IsEmpty);
        }

        [TestCategory("Array2DExtensions")]
        [TestMethod]
        public void Test_Array2DExtensions_AsSpan_Populated()
        {
            int[,] array =
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 }
            };

            Span<int> span = array.AsSpan();

            Assert.AreEqual(span.Length, array.Length);

            ref int r0 = ref array[0, 0];
            ref int r1 = ref span[0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }
#endif
    }
}
