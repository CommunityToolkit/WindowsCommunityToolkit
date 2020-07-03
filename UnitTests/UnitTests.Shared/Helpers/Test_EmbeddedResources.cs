// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Helpers
{
    [TestClass]
    public class Test_EmbeddedResources
    {
        private const string SampleText = "Hello world!";

        [TestCategory("EmbeddedResources")]
        [TestMethod]
        [DataRow("/Assets/SampleFile.txt")]
        [DataRow("\\Assets\\SampleFile.txt")]
        [DataRow("Assets/SampleFile.txt")]
        [DataRow("Assets\\SampleFile.txt")]
        [DataRow("/Assets\\SampleFile.txt")]
        [DataRow("\\Assets/SampleFile.txt")]
        public void Test_EmbeddedResources_Ok(string path)
        {
            Assert.AreEqual(SampleText, EmbeddedResources.GetString(path));
        }

        [TestCategory("EmbeddedResources")]
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [DataRow("SampleFile.txt")]
        [DataRow("/Assets/SampleFile")]
        public void Test_EmbeddedResources_Fail(string path)
        {
            Assert.AreEqual(SampleText, EmbeddedResources.GetString(path));
        }
    }
}
