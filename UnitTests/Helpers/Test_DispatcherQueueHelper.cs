// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.System;

namespace UnitTests.Helpers
{
    [TestClass]
    [Ignore("Ignored until issue on .Net Native is fixed. These are working.")]
    public class Test_DispatcherQueueHelper
    {
        private const int TIME_OUT = 5000;

        [TestCategory("Helpers")]
        [UITestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_DispatcherQueueHelper_Action_Null()
        {
            DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), default(Action));
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        public void Test_DispatcherQueueHelper_Action_Ok_UIThread()
        {
            DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), () =>
            {
                var textBlock = new TextBlock { Text = nameof(Test_DispatcherQueueHelper_Action_Ok_UIThread) };

                Assert.AreEqual(textBlock.Text, nameof(Test_DispatcherQueueHelper_Action_Ok_UIThread));
            }).Wait();
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_DispatcherQueueHelper_Action_Ok_NonUIThread()
        {
            var taskSource = new TaskCompletionSource<object>();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
                        await Task.Run(async () =>
                        {
                            await DispatcherQueueHelper.ExecuteOnUIThreadAsync(dispatcherQueue, () =>
                            {
                                var textBlock = new TextBlock { Text = nameof(Test_DispatcherQueueHelper_Action_Ok_NonUIThread) };

                                Assert.AreEqual(textBlock.Text, nameof(Test_DispatcherQueueHelper_Action_Ok_NonUIThread));

                                taskSource.SetResult(null);
                            });
                        });
                    }
                    catch (Exception e)
                    {
                        taskSource.SetException(e);
                    }
                });
            await taskSource.Task;
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        public void Test_DispatcherQueueHelper_Action_Exception()
        {
            var task = DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), () =>
            {
                throw new ArgumentException(nameof(this.Test_DispatcherQueueHelper_Action_Exception));
            });

            Assert.IsNotNull(task);
            Assert.AreEqual(task.Status, TaskStatus.Faulted);
            Assert.IsNotNull(task.Exception);
            Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(ArgumentException));
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_DispatcherQueueHelper_FuncOfT_Null()
        {
            DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), default(Func<int>));
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        public void Test_DispatcherQueueHelper_FuncOfT_Ok_UIThread()
        {
            var textBlock = DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), () =>
            {
                return new TextBlock { Text = nameof(Test_DispatcherQueueHelper_FuncOfT_Ok_UIThread) };
            }).Result;

            Assert.AreEqual(textBlock.Text, nameof(Test_DispatcherQueueHelper_FuncOfT_Ok_UIThread));
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_DispatcherQueueHelper_FuncOfT_Ok_NonUIThread()
        {
            var taskSource = new TaskCompletionSource<object>();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
                        await Task.Run(async () =>
                        {
                            var textBlock = await DispatcherQueueHelper.ExecuteOnUIThreadAsync(dispatcherQueue, () =>
                            {
                                return new TextBlock { Text = nameof(Test_DispatcherQueueHelper_FuncOfT_Ok_NonUIThread) };
                            });
                            await DispatcherQueueHelper.ExecuteOnUIThreadAsync(dispatcherQueue, () =>
                            {
                                Assert.AreEqual(textBlock.Text, nameof(Test_DispatcherQueueHelper_FuncOfT_Ok_NonUIThread));
                                taskSource.SetResult(null);
                            });
                        });
                    }
                    catch (Exception e)
                    {
                        taskSource.SetException(e);
                    }
                });
            await taskSource.Task;
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        public void Test_DispatcherQueueHelper_FuncOfT_Exception()
        {
            var task = DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), new Func<int>(() =>
            {
                throw new ArgumentException(nameof(this.Test_DispatcherQueueHelper_FuncOfT_Exception));
            }));

            Assert.IsNotNull(task);
            Assert.AreEqual(task.Status, TaskStatus.Faulted);
            Assert.IsNotNull(task.Exception);
            Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(ArgumentException));
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [SuppressMessage("Style", "IDE0034", Justification = "Explicit overload for clarity")]
        public void Test_DispatcherQueueHelper_FuncOfTask_Null()
        {
            DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), default(Func<Task>));
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        public void Test_DispatcherQueueHelper_FuncOfTask_Ok_UIThread()
        {
            DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), () =>
            {
                var textBlock = new TextBlock { Text = nameof(Test_DispatcherQueueHelper_FuncOfTask_Ok_UIThread) };

                Assert.AreEqual(textBlock.Text, nameof(Test_DispatcherQueueHelper_FuncOfTask_Ok_UIThread));

                return Task.CompletedTask;
            }).Wait();
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_DispatcherQueueHelper_FuncOfTask_Ok_NonUIThread()
        {
            var taskSource = new TaskCompletionSource<object>();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        await DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), async () =>
                        {
                            await Task.Yield();

                            var textBlock = new TextBlock { Text = nameof(Test_DispatcherQueueHelper_FuncOfTask_Ok_NonUIThread) };

                            Assert.AreEqual(textBlock.Text, nameof(Test_DispatcherQueueHelper_FuncOfTask_Ok_NonUIThread));

                            taskSource.SetResult(null);
                        });
                    }
                    catch (Exception e)
                    {
                        taskSource.SetException(e);
                    }
                });
            await taskSource.Task;
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        public void Test_DispatcherQueueHelper_FuncOfTask_Exception()
        {
            var task = DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), new Func<Task>(() =>
            {
                throw new ArgumentException(nameof(this.Test_DispatcherQueueHelper_FuncOfTask_Exception));
            }));

            Assert.IsNotNull(task);
            Assert.AreEqual(task.Status, TaskStatus.Faulted);
            Assert.IsNotNull(task.Exception);
            Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(ArgumentException));
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_DispatcherQueueHelper_FuncOfTaskOfT_Null()
        {
            DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), default(Func<Task<int>>));
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        public void Test_DispatcherQueueHelper_FuncOfTaskOfT_Ok_UIThread()
        {
            DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), () =>
            {
                var textBlock = new TextBlock { Text = nameof(Test_DispatcherQueueHelper_FuncOfTaskOfT_Ok_UIThread) };

                Assert.AreEqual(textBlock.Text, nameof(Test_DispatcherQueueHelper_FuncOfTaskOfT_Ok_UIThread));

                return Task.FromResult(1);
            }).Wait();
        }

        [TestCategory("Helpers")]
        [TestMethod]
        public async Task Test_DispatcherQueueHelper_FuncOfTaskOfT_Ok_NonUIThread()
        {
            var taskSource = new TaskCompletionSource<object>();
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                CoreDispatcherPriority.Normal, async () =>
                {
                    try
                    {
                        await DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), async () =>
                        {
                            await Task.Yield();

                            var textBlock = new TextBlock { Text = nameof(Test_DispatcherQueueHelper_FuncOfTaskOfT_Ok_NonUIThread) };

                            Assert.AreEqual(textBlock.Text, nameof(Test_DispatcherQueueHelper_FuncOfTaskOfT_Ok_NonUIThread));

                            taskSource.SetResult(null);

                            return textBlock;
                        });
                    }
                    catch (Exception e)
                    {
                        taskSource.SetException(e);
                    }
                });
            await taskSource.Task;
        }

        [TestCategory("Helpers")]
        [UITestMethod]
        public void Test_DispatcherQueueHelper_FuncOfTaskOfT_Exception()
        {
            var task = DispatcherQueueHelper.ExecuteOnUIThreadAsync(DispatcherQueue.GetForCurrentThread(), new Func<Task<int>>(() =>
            {
                throw new ArgumentException(nameof(this.Test_DispatcherQueueHelper_FuncOfTaskOfT_Exception));
            }));

            Assert.IsNotNull(task);
            Assert.AreEqual(task.Status, TaskStatus.Faulted);
            Assert.IsNotNull(task.Exception);
            Assert.IsInstanceOfType(task.Exception.InnerExceptions.FirstOrDefault(), typeof(ArgumentException));
        }
    }
}
