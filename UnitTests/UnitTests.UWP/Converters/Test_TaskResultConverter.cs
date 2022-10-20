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
        public void Test_TaskResultConverter_Instance_Int32()
        {
            TaskResultConverter converter = new();

            TaskCompletionSource<int> tcs = new();

            Assert.AreEqual(null, converter.Convert(tcs.Task, null, null, null));

            tcs.SetCanceled();

            Assert.AreEqual(null, converter.Convert(tcs.Task, null, null, null));

            tcs = new TaskCompletionSource<int>();

            tcs.SetException(new InvalidOperationException("Test"));

            Assert.AreEqual(null, converter.Convert(tcs.Task, null, null, null));

            tcs = new TaskCompletionSource<int>();

            tcs.SetResult(42);

            Assert.AreEqual(42, converter.Convert(tcs.Task, null, null, null));
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_TaskResultConverter_Instance_String()
        {
            TaskResultConverter converter = new();

            TaskCompletionSource<string> tcs = new();

            Assert.AreEqual(null, converter.Convert(tcs.Task, null, null, null));

            tcs.SetCanceled();

            Assert.AreEqual(null, converter.Convert(tcs.Task, null, null, null));

            tcs = new TaskCompletionSource<string>();

            tcs.SetException(new InvalidOperationException("Test"));

            Assert.AreEqual(null, converter.Convert(tcs.Task, null, null, null));

            tcs = new TaskCompletionSource<string>();

            tcs.SetResult("Hello world");

            Assert.AreEqual("Hello world", converter.Convert(tcs.Task, null, null, null));
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_TaskResultConverter_Instance_UnsetValue()
        {
            TaskResultConverter converter = new();

            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert(null, null, null, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, converter.Convert("Hello world", null, null, null));
        }

        [TestCategory("Converters")]
        [UITestMethod]
        public void Test_TaskResultConverter_Instance_Null()
        {
            TaskResultConverter converter = new();

            CancellationTokenSource cts = new();

            cts.Cancel();

            Assert.AreEqual(null, converter.Convert(Task.FromCanceled(cts.Token), null, null, null));
            Assert.AreEqual(null, converter.Convert(Task.FromException(new Exception()), null, null, null));
            Assert.AreEqual(null, converter.Convert(Task.CompletedTask, null, null, null));

            TaskCompletionSource<int> tcs1 = new();

            Assert.AreEqual(null, converter.Convert(tcs1.Task, null, null, null));

            TaskCompletionSource<string> tcs2 = new();

            Assert.AreEqual(null, converter.Convert(tcs2.Task, null, null, null));
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