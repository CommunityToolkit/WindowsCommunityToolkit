// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace UnitTests.XamlIslands.UWPApp
{
    public sealed partial class TestsPage
    {
        public TestsPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await RunTestsAsync();
        }

        public async Task RunTestsAsync()
        {
            richTextBlock.Blocks.Clear();
            App.Dispatcher = DispatcherQueue.GetForCurrentThread();
            App.XamlRoot = XamlRoot;

            await Task.Run(async () =>
            {
                TestResult testResult = default;

                Stopwatch sw = new Stopwatch();

                await WriteLineAsync("--- Starting Tests Execution ---");

                sw.Start();

                foreach (var testClass in typeof(XamlIslandsTest_SystemInformation).Assembly.GetTypes())
                {
                    Attribute[] attributes = Attribute.GetCustomAttributes(testClass);

                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is STATestClassAttribute || attribute is TestClassAttribute)
                        {
                            var partialTestResult = await RunTestsAsync(testClass);
                            testResult += partialTestResult;
                            break;
                        }
                    }
                }

                sw.Stop();

                await WriteLineAsync($"--- Finished Tests Execution ({testResult.Passed}/{testResult.Count}) ---", testResult.Failed > 0 ? Colors.Red : Colors.Green);
                await WriteLineAsync($"--- Duration - {sw.Elapsed} ---");
            });
        }

        private async Task<TestResult> RunTestsAsync(Type type)
        {
            int count = 0;
            int passed = 0;
            var initMethod = GetFirstMethod(type, typeof(TestInitializeAttribute));
            var cleanupMethod = GetFirstMethod(type, typeof(TestCleanupAttribute));

            foreach (var method in type.GetMethods())
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(method);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is STATestMethodAttribute || attribute is TestMethodAttribute)
                    {
                        count++;
                        try
                        {
                            await WriteLineAsync($"{type.FullName}.{method.Name}\t - \t Running...");

                            var instance = Activator.CreateInstance(type);

                            if (initMethod != null)
                            {
                                var resultInit = initMethod.Invoke(instance, null);
                                if (resultInit is Task taskInit)
                                {
                                    await taskInit;
                                }
                            }

                            var result = method.Invoke(instance, null);
                            if (result is Task t)
                            {
                                await t;
                            }

                            if (cleanupMethod != null)
                            {
                                var resultCleanup = cleanupMethod.Invoke(instance, null);
                                if (resultCleanup is Task taskCleanup)
                                {
                                    await taskCleanup;
                                }
                            }

                            await WriteLineAsync($"{type.FullName}.{method.Name}\t - \t PASS      ", Colors.Green, true);
                            passed++;
                        }
                        catch (Exception ex)
                        {
                            await WriteLineAsync($"{type.FullName}.{method.Name}\t - \t FAIL      :{Environment.NewLine}{ex}", Colors.Red, true);
                        }

                        break;
                    }
                }
            }

            return new TestResult(count, passed);
        }

        private Task WriteLineAsync(string message, Color? color = null, bool deleteLastLine = false)
        {
            Debug.WriteLine(message);
            return App.Dispatcher.ExecuteOnUIThreadAsync(() =>
            {
                if (deleteLastLine)
                {
                    richTextBlock.Blocks.Remove(richTextBlock.Blocks.Last());
                }

                var paragraph = new Paragraph();
                if (color != null)
                {
                    paragraph.Foreground = new SolidColorBrush(color.Value);
                }

                paragraph.Inlines.Add(new Run
                {
                    Text = message
                });
                richTextBlock.Blocks.Add(paragraph);
            });
        }

        private struct TestResult
        {
            public int Count { get; }

            public int Passed { get; }

            public int Failed => Count - Passed;

            public TestResult(int count, int passed)
            {
                Count = count;
                Passed = passed;
            }

            public static TestResult operator +(TestResult a, TestResult b) => new TestResult(a.Count + b.Count, a.Passed + b.Passed);
        }

        private MethodInfo GetFirstMethod(Type ofType, Type attributeType)
        {
            foreach (var method in ofType.GetMethods())
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(method);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute.GetType() == attributeType)
                    {
                        return method;
                    }
                }
            }

            return null;
        }
    }
}
