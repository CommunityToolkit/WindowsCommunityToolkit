// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.HighPerformance.Streams
{
    public partial class Test_MemoryStream
    {
        [TestCategory("ParallelHelper")]
        [TestMethod]
        public void Test_ParallelHelper_ParameterName_ThrowArgumentExceptionForSeekOrigin()
        {
            var stream = new byte[10].AsMemory().AsStream();

            try
            {
                stream.Seek(0, (SeekOrigin)int.MinValue);
            }
            catch (ArgumentException e)
            {
                var method = stream.GetType().GetMethod(nameof(Stream.Seek));
                var name = method!.GetParameters()[1].Name;

                Assert.AreEqual(e.ParamName, name);

                return;
            }

            Assert.Fail("Failed to raise correct exception");
        }
    }
}
