// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Win32.UI.XamlHost;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using System.Reflection;
using Windows.System;
using System.Diagnostics;

namespace UnitTests.XamlIslands
{
    class Program
    {
        private static IXamlMetadataContainer _metadataContainer;
        internal static DispatcherQueue _dispatcher;

        [STAThread]
        public static void Main()
        {
            _metadataContainer = XamlApplicationExtensions.GetOrCreateXamlMetadataContainer();

            using (var xamlSource = new DesktopWindowXamlSource())
            {
                var frame = UWPTypeFactory.CreateXamlContentByType("Windows.UI.Xaml.Controls.Frame");
                xamlSource.Content = frame;

                _dispatcher = DispatcherQueue.GetForCurrentThread();

                _dispatcher.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
                {
                    TestResult testResult = default;

                    Stopwatch sw = new Stopwatch();

                    ConsoleWriteLineColor("--- Starting Tests Execution ---");

                    sw.Start();

                    foreach (var testClass in typeof(XamlIslandsTest_ThemeListener_Threading).Assembly.GetTypes())
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

                    var color = testResult.Failed == 0 ? ConsoleColor.Green : ConsoleColor.Red;

                    ConsoleWriteLineColor($"--- Finished Tests Execution ({testResult.Passed}/{testResult.Count}) ---", color);
                    ConsoleWriteLineColor($"--- Duration - {sw.Elapsed} ---");

                    Window.Current.CoreWindow.Close();
                    System.Windows.Application.Current.Shutdown();
                });

                // This is just to have a Win32 message loop so the dispatcher processes it's events. This is not WPF or WinForms specific.
                var app = new System.Windows.Application();
                app.Run();
            }
        }

        private static async Task<TestResult> RunTestsAsync(Type type)
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

                            TestPass(type, method);
                            passed++;
                        }
                        catch (Exception ex)
                        {
                            TestFail(type, method, ex);
                        }
                        break;
                    }
                }
            }

            return new TestResult(count, passed);
        }

        struct TestResult
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

        private static void TestPass(Type type, MethodInfo method)
        {
            ConsoleWriteLineColor($"{type.FullName}.{method.Name}\t - \tPASS", ConsoleColor.Green);
        }

        private static void TestFail(Type type, MethodInfo method, Exception ex)
        {
            ConsoleWriteLineColor($"{type.FullName}.{method.Name}\t - \tFAIL:{Environment.NewLine}{ex}", ConsoleColor.Red);
        }

        private static void ConsoleWriteLineColor(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static MethodInfo GetFirstMethod(Type ofType, Type attributeType)
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
