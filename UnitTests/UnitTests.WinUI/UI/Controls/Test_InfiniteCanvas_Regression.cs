// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using FluentAssertions;
using CommunityToolkit.WinUI.Helpers;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunityToolkit.WinUI;
using Microsoft.UI.Dispatching;

namespace UnitTests.WinUI.UI.Controls
{
    [TestClass]
    public class Test_InfiniteCanvas_Regression
    {
        [TestCategory("InfiniteCanvas")]
        [TestMethod]
        [DataRow(@"Assets\Samples\InfiniteCanvasExportPreMedia.json", DisplayName = "Version1")]
        [DataRow(@"Assets\Samples\InfiniteCanvasExport.json", DisplayName = "Version2")]

        public async Task Test_InfiniteCanvas_LoadsFile(string file)
        {
            var taskSource = new TaskCompletionSource<object>();
            await App.DispatcherQueue.EnqueueAsync(
                async () => 
                {
                    try
                    {
                        string json = await StorageFileHelper.ReadTextFromPackagedFileAsync(file);

                        InfiniteCanvasVirtualDrawingSurface.LoadJson(json).Should().NotBeEmpty();

                        taskSource.SetResult(null);
                    }
                    catch (Exception e)
                    {
                        taskSource.SetException(e);
                    }
                }, DispatcherQueuePriority.Normal);
            await taskSource.Task;
        }
    }
}