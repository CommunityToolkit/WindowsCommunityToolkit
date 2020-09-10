// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.Toolkit.HighPerformance.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Memory
{
    [TestClass]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Test class for generic type")]
    public class Test_Memory2DT
    {
        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Empty()
        {
            Memory2D<int> empty1 = default;

            Assert.IsTrue(empty1.IsEmpty);
            Assert.AreEqual(empty1.Size, 0);
            Assert.AreEqual(empty1.Width, 0);
            Assert.AreEqual(empty1.Height, 0);

            Memory2D<string> empty2 = Memory2D<string>.Empty;

            Assert.IsTrue(empty2.IsEmpty);
            Assert.AreEqual(empty2.Size, 0);
            Assert.AreEqual(empty2.Width, 0);
            Assert.AreEqual(empty2.Height, 0);
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Array1DConstructor()
        {
            int[] array =
            {
                1, 2, 3, 4, 5, 6
            };

            Memory2D<int> memory2d = new Memory2D<int>(array, 1, 2, 2, 1);

            Assert.IsFalse(memory2d.IsEmpty);
            Assert.AreEqual(memory2d.Size, 4);
            Assert.AreEqual(memory2d.Width, 2);
            Assert.AreEqual(memory2d.Height, 2);
            Assert.AreEqual(memory2d.Span[0, 0], 2);
            Assert.AreEqual(memory2d.Span[1, 1], 6);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new Memory2D<object>(new string[1], 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, -99, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, 0, -10, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, 0, 1, 1, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, 0, 1, -100, 1));
            Assert.ThrowsException<ArgumentException>(() => new Memory2D<int>(array, 0, 10, 1, 120));
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Array2DConstructor_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array);

            Assert.IsFalse(memory2d.IsEmpty);
            Assert.AreEqual(memory2d.Size, 6);
            Assert.AreEqual(memory2d.Width, 3);
            Assert.AreEqual(memory2d.Height, 2);
            Assert.AreEqual(memory2d.Span[0, 1], 2);
            Assert.AreEqual(memory2d.Span[1, 2], 6);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new Memory2D<object>(new string[1, 2]));
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Array2DConstructor_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array, 0, 1, 2, 2);

            Assert.IsFalse(memory2d.IsEmpty);
            Assert.AreEqual(memory2d.Size, 4);
            Assert.AreEqual(memory2d.Width, 2);
            Assert.AreEqual(memory2d.Height, 2);
            Assert.AreEqual(memory2d.Span[0, 0], 2);
            Assert.AreEqual(memory2d.Span[1, 1], 6);

            Assert.ThrowsException<ArrayTypeMismatchException>(() => new Memory2D<object>(new string[1, 2], 0, 0, 2, 2));
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Array3DConstructor_1()
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

            Memory2D<int> memory2d = new Memory2D<int>(array, 1);

            Assert.IsFalse(memory2d.IsEmpty);
            Assert.AreEqual(memory2d.Size, 6);
            Assert.AreEqual(memory2d.Width, 3);
            Assert.AreEqual(memory2d.Height, 2);
            Assert.AreEqual(memory2d.Span[0, 1], 20);
            Assert.AreEqual(memory2d.Span[1, 2], 60);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, 20));
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Array3DConstructor_2()
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

            Memory2D<int> memory2d = new Memory2D<int>(array, 1, 0, 1, 2, 2);

            Assert.IsFalse(memory2d.IsEmpty);
            Assert.AreEqual(memory2d.Size, 4);
            Assert.AreEqual(memory2d.Width, 2);
            Assert.AreEqual(memory2d.Height, 2);
            Assert.AreEqual(memory2d.Span[0, 0], 20);
            Assert.AreEqual(memory2d.Span[1, 1], 60);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, -1, 1, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, 1, -1, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, 1, 1, -1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, 1, 1, 1, -1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array, 1, 1, 1, 1, -1));
        }

#if !WINDOWS_UWP
        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_MemoryConstructor()
        {
            Memory<int> memory = new[]
            {
                1, 2, 3, 4, 5, 6
            };

            Memory2D<int> memory2d = memory.AsMemory2D(1, 2, 2, 1);

            Assert.IsFalse(memory2d.IsEmpty);
            Assert.AreEqual(memory2d.Size, 4);
            Assert.AreEqual(memory2d.Width, 2);
            Assert.AreEqual(memory2d.Height, 2);
            Assert.AreEqual(memory2d.Span[0, 0], 2);
            Assert.AreEqual(memory2d.Span[1, 1], 6);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => memory.AsMemory2D(-99, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => memory.AsMemory2D(0, -10, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => memory.AsMemory2D(0, 1, 1, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => memory.AsMemory2D(0, 1, -100, 1));
            Assert.ThrowsException<ArgumentException>(() => memory.AsMemory2D(0, 10, 1, 120));
        }
#endif

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Slice_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array);

            Memory2D<int> slice1 = memory2d.Slice(1, 1, 1, 2);

            Assert.AreEqual(slice1.Size, 2);
            Assert.AreEqual(slice1.Height, 1);
            Assert.AreEqual(slice1.Width, 2);
            Assert.AreEqual(slice1.Span[0, 0], 5);
            Assert.AreEqual(slice1.Span[0, 1], 6);

            Memory2D<int> slice2 = memory2d.Slice(0, 1, 2, 2);

            Assert.AreEqual(slice2.Size, 4);
            Assert.AreEqual(slice2.Height, 2);
            Assert.AreEqual(slice2.Width, 2);
            Assert.AreEqual(slice2.Span[0, 0], 2);
            Assert.AreEqual(slice2.Span[1, 0], 5);
            Assert.AreEqual(slice2.Span[1, 1], 6);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array).Slice(-1, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array).Slice(1, -1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array).Slice(1, 1, 1, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array).Slice(1, 1, -1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array).Slice(10, 1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array).Slice(1, 12, 1, 12));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Memory2D<int>(array).Slice(1, 1, 55, 1));
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Slice_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array);

            Memory2D<int> slice1 = memory2d.Slice(0, 0, 2, 2);

            Assert.AreEqual(slice1.Size, 4);
            Assert.AreEqual(slice1.Height, 2);
            Assert.AreEqual(slice1.Width, 2);
            Assert.AreEqual(slice1.Span[0, 0], 1);
            Assert.AreEqual(slice1.Span[1, 1], 5);

            Memory2D<int> slice2 = slice1.Slice(1, 0, 1, 2);

            Assert.AreEqual(slice2.Size, 2);
            Assert.AreEqual(slice2.Height, 1);
            Assert.AreEqual(slice2.Width, 2);
            Assert.AreEqual(slice2.Span[0, 0], 4);
            Assert.AreEqual(slice2.Span[0, 1], 5);

            Memory2D<int> slice3 = slice2.Slice(0, 1, 1, 1);

            Assert.AreEqual(slice3.Size, 1);
            Assert.AreEqual(slice3.Height, 1);
            Assert.AreEqual(slice3.Width, 1);
            Assert.AreEqual(slice3.Span[0, 0], 5);
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_TryGetMemory_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array);

            bool success = memory2d.TryGetMemory(out Memory<int> memory);

            Assert.IsFalse(success);
            Assert.IsTrue(memory.IsEmpty);
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_TryGetMemory_2()
        {
            int[] array = { 1, 2, 3, 4 };

            Memory2D<int> memory2d = new Memory2D<int>(array, 2, 2);

            bool success = memory2d.TryGetMemory(out Memory<int> memory);

            Assert.IsTrue(success);
            Assert.AreEqual(memory.Length, array.Length);
            Assert.AreEqual(memory.Span[2], 3);
        }

#if !WINDOWS_UWP
        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_TryGetMemory_3()
        {
            Memory<int> data = new[] { 1, 2, 3, 4 };

            Memory2D<int> memory2d = data.AsMemory2D(2, 2);

            bool success = memory2d.TryGetMemory(out Memory<int> memory);

            Assert.IsTrue(success);
            Assert.AreEqual(memory.Length, data.Length);
            Assert.AreEqual(memory.Span[2], 3);
        }
#endif

        [TestCategory("Memory2DT")]
        [TestMethod]
        public unsafe void Test_Memory2DT_Pin_1()
        {
            int[] array = { 1, 2, 3, 4 };

            Memory2D<int> memory2d = new Memory2D<int>(array, 2, 2);

            using var pin = memory2d.Pin();

            Assert.AreEqual(((int*)pin.Pointer)[0], 1);
            Assert.AreEqual(((int*)pin.Pointer)[3], 4);
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public unsafe void Test_Memory2DT_Pin_2()
        {
            int[] array = { 1, 2, 3, 4 };

            Memory2D<int> memory2d = new Memory2D<int>(array, 2, 2);

            using var pin = memory2d.Pin();

            Assert.AreEqual(((int*)pin.Pointer)[0], 1);
            Assert.AreEqual(((int*)pin.Pointer)[3], 4);
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_ToArray_1()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array);

            int[,] copy = memory2d.ToArray();

            Assert.AreEqual(copy.GetLength(0), array.GetLength(0));
            Assert.AreEqual(copy.GetLength(1), array.GetLength(1));

            CollectionAssert.AreEqual(array, copy);
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_ToArray_2()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array, 0, 0, 2, 2);

            int[,] copy = memory2d.ToArray();

            Assert.AreEqual(copy.GetLength(0), 2);
            Assert.AreEqual(copy.GetLength(1), 2);

            int[,] expected =
            {
                { 1, 2 },
                { 4, 5 }
            };

            CollectionAssert.AreEqual(expected, copy);
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_Equals()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array);

            Assert.IsFalse(memory2d.Equals(null));
            Assert.IsFalse(memory2d.Equals(new Memory2D<int>(array, 0, 1, 2, 2)));
            Assert.IsTrue(memory2d.Equals(new Memory2D<int>(array)));
            Assert.IsTrue(memory2d.Equals(memory2d));

            ReadOnlyMemory2D<int> readOnlyMemory2d = memory2d;

            Assert.IsTrue(memory2d.Equals(readOnlyMemory2d));
            Assert.IsFalse(memory2d.Equals(readOnlyMemory2d.Slice(0, 1, 2, 2)));
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_GetHashCode()
        {
            Assert.AreEqual(Memory2D<int>.Empty.GetHashCode(), 0);

            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array);

            int a = memory2d.GetHashCode(), b = memory2d.GetHashCode();

            Assert.AreEqual(a, b);

            int c = new Memory2D<int>(array, 0, 1, 2, 2).GetHashCode();

            Assert.AreNotEqual(a, c);
        }

        [TestCategory("Memory2DT")]
        [TestMethod]
        public void Test_Memory2DT_ToString()
        {
            int[,] array =
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            Memory2D<int> memory2d = new Memory2D<int>(array);

            string text = memory2d.ToString();

            const string expected = "Microsoft.Toolkit.HighPerformance.Memory.Memory2D<System.Int32>[2, 3]";

            Assert.AreEqual(text, expected);
        }
    }
}