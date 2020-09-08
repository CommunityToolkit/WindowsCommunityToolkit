// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_TypeExtensions
    {
        [TestCategory("TypeExtensions")]
        [TestMethod]
        [DataRow("bool", typeof(bool))]
        [DataRow("int", typeof(int))]
        [DataRow("float", typeof(float))]
        [DataRow("double", typeof(double))]
        [DataRow("decimal", typeof(decimal))]
        [DataRow("object", typeof(object))]
        [DataRow("string", typeof(string))]
        public void Test_TypeExtensions_BuiltInTypes(string name, Type type)
        {
            Assert.AreEqual(name, type.ToTypeString());
        }

        [TestCategory("TypeExtensions")]
        [TestMethod]
        [DataRow("int?", typeof(int?))]
        [DataRow("System.DateTime?", typeof(DateTime?))]
        [DataRow("(int, float)", typeof((int, float)))]
        [DataRow("(double?, string, int)?", typeof((double?, string, int)?))]
        [DataRow("int[]", typeof(int[]))]
        [DataRow("int[,]", typeof(int[,]))]
        [DataRow("System.Span<float>", typeof(Span<float>))]
        [DataRow("System.Memory<char>", typeof(Memory<char>))]
        [DataRow("System.Collections.Generic.IEnumerable<int>", typeof(IEnumerable<int>))]
        [DataRow("System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<float>>", typeof(Dictionary<int, List<float>>))]
        public void Test_TypeExtensions_GenericTypes(string name, Type type)
        {
            Assert.AreEqual(name, type.ToTypeString());
        }
    }
}
