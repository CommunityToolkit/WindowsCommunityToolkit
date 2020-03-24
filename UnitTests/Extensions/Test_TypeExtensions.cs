// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Toolkit.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_TypeExtensions
    {
        [TestCategory("TypeExtensions")]
        [TestMethod]
        public void Test_TypeExtensions_BuiltInTypes()
        {
            Assert.AreEqual("bool", typeof(bool).ToTypeString());
            Assert.AreEqual("int", typeof(int).ToTypeString());
            Assert.AreEqual("float", typeof(float).ToTypeString());
            Assert.AreEqual("double", typeof(double).ToTypeString());
            Assert.AreEqual("decimal", typeof(decimal).ToTypeString());
            Assert.AreEqual("object", typeof(object).ToTypeString());
            Assert.AreEqual("string", typeof(string).ToTypeString());
        }

        [TestCategory("TypeExtensions")]
        [TestMethod]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1009", Justification = "Nullable value tuple type")]
        public void Test_TypeExtensions_GenericTypes()
        {
            Assert.AreEqual("int?", typeof(int?).ToTypeString());
            Assert.AreEqual("System.DateTime?", typeof(DateTime?).ToTypeString());
            Assert.AreEqual("(int, float)", typeof((int, float)).ToTypeString());
            Assert.AreEqual("(double?, string, int)?", typeof((double?, string, int)?).ToTypeString());
            Assert.AreEqual("int[]", typeof(int[]).ToTypeString());
            Assert.AreEqual(typeof(int[,]).ToTypeString(), "int[,]");
            Assert.AreEqual("System.Span<float>", typeof(Span<float>).ToTypeString());
            Assert.AreEqual("System.Memory<char>", typeof(Memory<char>).ToTypeString());
            Assert.AreEqual("System.Collections.Generic.IEnumerable<int>", typeof(IEnumerable<int>).ToTypeString());
            Assert.AreEqual(typeof(Dictionary<int, List<float>>).ToTypeString(), "System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<float>>");
        }
    }
}
