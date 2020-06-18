// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Memory
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_Span2DT
    {
        [TestCategory("Span2DT")]
        [TestMethod]
        public void Test_Span2DT_Empty()
        {
            Span2D<int> empty1 = default;

            Assert.IsTrue(empty1.IsEmpty);
            Assert.AreEqual(empty1.Size, 0);
            Assert.AreEqual(empty1.Width, 0);
            Assert.AreEqual(empty1.Height, 0);

            Span2D<string> empty2 = Span2D<string>.Empty;

            Assert.IsTrue(empty2.IsEmpty);
            Assert.AreEqual(empty2.Size, 0);
            Assert.AreEqual(empty2.Width, 0);
            Assert.AreEqual(empty2.Height, 0);
        }

#if !WINDOWS_UWP
        [TestCategory("Span2DT")]
        [TestMethod]
        public unsafe void Test_Span2DT_RefConstructor()
        {
            Span<int> span = stackalloc[]
            {
                1, 2, 3, 4, 5, 6
            };

            Span2D<int> span2d = new Span2D<int>(ref span[0], 2, 3, 0);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 6);
            Assert.AreEqual(span2d.Width, 3);
            Assert.AreEqual(span2d.Height, 2);

            span2d[0, 0] = 99;
            span2d[1, 2] = 101;

            Assert.AreEqual(span[0], 99);
            Assert.AreEqual(span[5], 101);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(ref Unsafe.AsRef<int>(null), -1, 0, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(ref Unsafe.AsRef<int>(null), 1, -2, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(ref Unsafe.AsRef<int>(null), 1, 0, -5));
        }

        [TestCategory("Span2DT")]
        [TestMethod]
        public unsafe void Test_Span2DT_PtrConstructor()
        {
            int* ptr = stackalloc[]
            {
                1, 2, 3, 4, 5, 6
            };

            Span2D<int> span2d = new Span2D<int>(ptr, 2, 3, 0);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 6);
            Assert.AreEqual(span2d.Width, 3);
            Assert.AreEqual(span2d.Height, 2);

            span2d[0, 0] = 99;
            span2d[1, 2] = 101;

            Assert.AreEqual(ptr[0], 99);
            Assert.AreEqual(ptr[5], 101);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>((void*)0, -1, 0, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>((void*)0, 1, -2, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>((void*)0, 1, 0, -5));
        }
#endif

        [TestCategory("Span2DT")]
        [TestMethod]
        public void Test_Span2DT_Array1DConstructor()
        {
            int[] array =
            {
                1, 2, 3, 4, 5, 6
            };

            Span2D<int> span2d = new Span2D<int>(array, 1, 2, 2, 1);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 4);
            Assert.AreEqual(span2d.Width, 2);
            Assert.AreEqual(span2d.Height, 2);

            span2d[0, 0] = 99;
            span2d[1, 1] = 101;

            Assert.AreEqual(array[1], 99);
            Assert.AreEqual(array[5], 101);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new Span2D<object>(new string[1], 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(array, -99, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(array, 0, -10, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(array, 0, 1, 1, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(array, 0, 1, -100, 1));
            Assert.ThrowsException<ArgumentException>(() => new Span2D<int>(array, 0, 10, 1, 120));
        }

        [TestCategory("Span2DT")]
        [TestMethod]
        public void Test_Span2DT_Array2DConstructor_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Span2D<int> span2d = new Span2D<int>(array);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 6);
            Assert.AreEqual(span2d.Width, 3);
            Assert.AreEqual(span2d.Height, 2);

            span2d[0, 1] = 99;
            span2d[1, 2] = 101;

            Assert.AreEqual(array[0, 1], 99);
            Assert.AreEqual(array[1, 2], 101);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new Span2D<object>(new string[1, 2]));
        }

        [TestCategory("Span2DT")]
        [TestMethod]
        public void Test_Span2DT_Array2DConstructor_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Span2D<int> span2d = new Span2D<int>(array, 0, 1, 2, 2);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 4);
            Assert.AreEqual(span2d.Width, 2);
            Assert.AreEqual(span2d.Height, 2);

            span2d[0, 0] = 99;
            span2d[1, 1] = 101;

            Assert.AreEqual(array[0, 1], 99);
            Assert.AreEqual(array[1, 2], 101);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new Span2D<object>(new string[1, 2], 0, 0, 2, 2));
        }

        [TestCategory("Span2DT")]
        [TestMethod]
        public void Test_Span2DT_Array3DConstructo()
        {
            int[,,] array =
            {
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 }
                },
                {
                    { 10, 20, 30 },
                    { 40, 50, 60 }
                }
            };

            Span2D<int> span2d = new Span2D<int>(array, 1);

            Assert.IsFalse(span2d.IsEmpty);
            Assert.AreEqual(span2d.Size, 6);
            Assert.AreEqual(span2d.Width, 3);
            Assert.AreEqual(span2d.Height, 2);

            span2d[0, 1] = 99;
            span2d[1, 2] = 101;

            Assert.AreEqual(span2d[0, 0], 10);
            Assert.AreEqual(array[1, 0, 1], 99);
            Assert.AreEqual(array[1, 1, 2], 101);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(array, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Span2D<int>(array, 20));
        }
    }
}