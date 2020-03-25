// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Extensions
{
    [TestClass]
    public partial class Test_ArrayExtensions
    {
        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_DangerousGetReference()
        {
            string[] tokens = "aa,bb,cc,dd,ee,ff,gg,hh,ii".Split(',');

            ref string r0 = ref Unsafe.AsRef(tokens.DangerousGetReference());
            ref string r1 = ref Unsafe.AsRef(tokens[0]);

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_DangerousGetReferenceAt_Zero()
        {
            string[] tokens = "aa,bb,cc,dd,ee,ff,gg,hh,ii".Split(',');

            ref string r0 = ref Unsafe.AsRef(tokens.DangerousGetReference());
            ref string r1 = ref Unsafe.AsRef(tokens.DangerousGetReferenceAt(0));

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }

        [TestCategory("ArrayExtensions")]
        [TestMethod]
        public void Test_ArrayExtensions_DangerousGetReferenceAt_Index()
        {
            string[] tokens = "aa,bb,cc,dd,ee,ff,gg,hh,ii".Split(',');

            ref string r0 = ref Unsafe.AsRef(tokens.DangerousGetReferenceAt(5));
            ref string r1 = ref Unsafe.AsRef(tokens[5]);

            Assert.IsTrue(Unsafe.AreSame(ref r0, ref r1));
        }
    }
}
