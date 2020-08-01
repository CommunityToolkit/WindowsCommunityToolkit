// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Extensions
{
    [TestClass]
    public class Test_TaskExtensions
    {
        [TestCategory("TaskExtensions")]
        [TestMethod]
        public void Test_TaskExtensions_ResultOrDefault()
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            Assert.AreEqual(null, ((Task)tcs.Task).GetResultOrDefault());

            tcs.SetCanceled();

            Assert.AreEqual(null, ((Task)tcs.Task).GetResultOrDefault());

            tcs = new TaskCompletionSource<int>();

            tcs.SetException(new InvalidOperationException("Test"));

            Assert.AreEqual(null, ((Task)tcs.Task).GetResultOrDefault());

            tcs = new TaskCompletionSource<int>();

            tcs.SetResult(42);

            Assert.AreEqual(42, ((Task)tcs.Task).GetResultOrDefault());
        }

        [TestCategory("TaskExtensions")]
        [TestMethod]
        public void Test_TaskExtensions_ResultOrDefault_OfT_Int32()
        {
            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            Assert.AreEqual(0, tcs.Task.GetResultOrDefault());

            tcs.SetCanceled();

            Assert.AreEqual(0, tcs.Task.GetResultOrDefault());

            tcs = new TaskCompletionSource<int>();

            tcs.SetException(new InvalidOperationException("Test"));

            Assert.AreEqual(0, tcs.Task.GetResultOrDefault());

            tcs = new TaskCompletionSource<int>();

            tcs.SetResult(42);

            Assert.AreEqual(42, tcs.Task.GetResultOrDefault());
        }

        [TestCategory("TaskExtensions")]
        [TestMethod]
        public void Test_TaskExtensions_ResultOrDefault_OfT_String()
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();

            Assert.AreEqual(null, tcs.Task.GetResultOrDefault());

            tcs.SetCanceled();

            Assert.AreEqual(null, tcs.Task.GetResultOrDefault());

            tcs = new TaskCompletionSource<string>();

            tcs.SetException(new InvalidOperationException("Test"));

            Assert.AreEqual(null, tcs.Task.GetResultOrDefault());

            tcs = new TaskCompletionSource<string>();

            tcs.SetResult("Hello world");

            Assert.AreEqual("Hello world", tcs.Task.GetResultOrDefault());
        }
    }
}
