// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    public partial class Test_ArrayExtensions
    {
        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_3D_DangerousGetReference_Int()
        {
            int[,,] array = new int[10, 20, 12];

            ref int r0 = ref array.DangerousGetReference();
            ref int r1 = ref array[0, 0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_3D_DangerousGetReference_String()
        {
            string[,,] array = new string[10, 20, 12];

            ref string r0 = ref array.DangerousGetReference();
            ref string r1 = ref array[0, 0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_3D_DangerousGetReferenceAt_Zero()
        {
            int[,,] array = new int[10, 20, 12];

            ref int r0 = ref array.DangerousGetReferenceAt(0, 0, 0);
            ref int r1 = ref array[0, 0, 0];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_3D_DangerousGetReferenceAt_Index()
        {
            int[,,] array = new int[10, 20, 12];

            ref int r0 = ref array.DangerousGetReferenceAt(5, 3, 4);
            ref int r1 = ref array[5, 3, 4];

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1312", Justification = "Dummy loop variable")]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501", Justification = "Empty test loop")]
        public void Test_ArrayExtensions_3D_GetRow_Rectangle()
        {
            int[,,] array = new int[10, 20, 12];

            int j = 0;
            foreach (ref int value in array.GetRow(4, 1))
            {
                Assert.IsTrue(Unsafe.AreSame(ref value, ref array[4, 1, j++]));
            }

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetRow(-1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetRow(20, -2));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetRow(29, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetRow(0, 55));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_3D_GetRow_Empty()
        {
            int[,,] array = new int[0, 0, 0];

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetRow(0, 0));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1312", Justification = "Dummy loop variable")]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1501", Justification = "Empty test loop")]
        public void Test_ArrayExtensions_3D_GetColumn_Rectangle()
        {
            int[,,] array = new int[10, 20, 12];

            int i = 0;
            foreach (ref int value in array.GetColumn(5, 1))
            {
                Assert.IsTrue(Unsafe.AreSame(ref value, ref array[5, i++, 1]));
            }

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetColumn(-1, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetColumn(0, -4));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetColumn(155, 0));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => array.GetColumn(0, 50));
        }
    }
}
