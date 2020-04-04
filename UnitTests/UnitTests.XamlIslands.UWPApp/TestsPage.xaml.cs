// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.XamlIslands.UWPApp
{
    public sealed partial class TestsPage
    {
        public TestsPage()
        {
            InitializeComponent();
        }

        public void RunTest(XamlRoot xamlRoot, Action testsDone)
        {
            XamlRoot = xamlRoot;

            Dispatcher = DispatcherQueue.GetForCurrentThread();

            _ = Task.Run(async () =>
            {
                TestResult testResult = default;

                Stopwatch sw = new Stopwatch();

                Debug.WriteLine("--- Starting Tests Execution ---");

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

                Debug.WriteLine($"--- Finished Tests Execution ({testResult.Passed}/{testResult.Count}) ---");
                Debug.WriteLine($"--- Duration - {sw.Elapsed} ---");

                testsDone?.Invoke();
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
                            Debug.WriteLine($"{type.FullName}.{method.Name}\t - \t Running...");

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

                            Debug.WriteLine($"{type.FullName}.{method.Name}\t - \t PASS      ");
                            passed++;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"{type.FullName}.{method.Name}\t - \t FAIL      :{Environment.NewLine}{ex}");
                        }

                        break;
                    }
                }
            }

            return new TestResult(count, passed);
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
