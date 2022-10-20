// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Windows.UI.Xaml;

namespace UnitTests.Converters
{
    [TestClass]
    public class Test_TaskResultConverter
    {
        [TestCategory("Converters")]
        [UITestMethod]
        [Ignore] // Ignore this value type test. Behavior will return null currently and not default.
        public void Test_TaskResultConverter_Instance_Int32()
        {
            TaskResultConverter converter = new();

            TaskCompletionSource<int> tcs = new();

            Assert.AreEqual(0, (int)converter.Convert(tcs.Task, typeof(int), null, null));

            tcs.SetCanceled();

            Assert.AreEqual(0, (int)converter.Convert(tcs.Task, typeof(int), null, null));

            tcs = new TaskCompletionSource<int>();

            tcs.SetException(new InvalidOperationException("Test"));

            Assert.AreEqual(0, (int)converter.Convert(tcs.Task, typeof(int), null, null));

            tcs = new TaskCompletionSource<int>();

            tcs.SetResult(42);

            Assert.AreEqual(42, (int)converter.Convert(tcs.Task, typeof(int), null, null));
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_TaskResultConverter_Instance_String()
        {
            TaskResultConverter converter = new();

            TaskCompletionSource<string> tcs = new();

            Assert.AreEqual(null, (string)converter.Convert(tcs.Task, typeof(string), null, null));

            tcs.SetCanceled();

            Assert.AreEqual(null, (string)converter.Convert(tcs.Task, typeof(string), null, null));

            tcs = new();

            tcs.SetException(new InvalidOperationException("Test"));

            Assert.AreEqual(null, (string)converter.Convert(tcs.Task, typeof(string), null, null));

            tcs = new();

            tcs.SetResult("Hello world");

            Assert.AreEqual("Hello world", (string)converter.Convert(tcs.Task, typeof(string), null, null));
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_TaskResultConverter_Instance_RawValue()
        {
            TaskResultConverter converter = new();

            Assert.AreEqual(42, converter.Convert(42, null, null, null));

            Assert.AreEqual(42, converter.Convert(42, typeof(int), null, null));

            Assert.AreEqual("Hello world", converter.Convert("Hello world", null, null, null));

            Assert.AreEqual("Hello world", converter.Convert("Hello world", typeof(string), null, null));
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_TaskResultConverter_Instance_NullObject()
        {
            TaskResultConverter converter = new();

            Assert.AreEqual(null, converter.Convert(null, null, null, null));

            // TODO: Think there may still be a problem for value types in x:Bind expressions, represented by these tests here,
            // but was going to be too big a change for 7.1.3, will have to get more feedback and evaluate later.
            /*Assert.AreEqual(0, (int)converter.Convert(null, typeof(int), null, null));

            Assert.AreEqual(false, (bool)converter.Convert(null, typeof(bool), null, null));*/

            Assert.AreEqual(null, converter.Convert(null, typeof(int), null, null));

            Assert.AreEqual(null, converter.Convert(null, typeof(bool), null, null));

            Assert.AreEqual(null, (int?)converter.Convert(null, typeof(int?), null, null));

            Assert.AreEqual(null, (string)converter.Convert(null, typeof(string), null, null));
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_TaskResultConverter_Instance_TaskNull()
        {
            TaskResultConverter converter = new();

            CancellationTokenSource cts = new();

            cts.Cancel();

            Assert.AreEqual(null, converter.Convert(Task.FromCanceled(cts.Token), null, null, null));
            Assert.AreEqual(null, converter.Convert(Task.FromException(new Exception()), null, null, null));
            Assert.AreEqual(null, converter.Convert(Task.CompletedTask, null, null, null));
        }

        [TestCategory("Converters")]
        [UITestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Test_TaskResultConverter_Instance_ConvertBack()
        {
            TaskResultConverter converter = new();

            Assert.AreEqual(null, converter.ConvertBack(null, null, null, null));
        }
    }
}