// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace UnitTests.UI.Controls
{
    [TestClass]
    public class Test_InfiniteCanvas_Regression
    {
        [TestCategory("InfiniteCanvas")]
        [TestMethod]
        public async Task Test_InfiniteCanvas_LoadsV1File()
        {
            var taskSource = new TaskCompletionSource<object>();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        string json = await StorageFileHelper.ReadTextFromPackagedFileAsync(@"Assets\Samples\InfiniteCanvasExport.json");

                        InfiniteCanvasVirtualDrawingSurface.LoadJson(json).Should().NotBeEmpty();

                        taskSource.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        taskSource.SetException(e);
                    }
                });
            await taskSource.Task;
        }
    }
}