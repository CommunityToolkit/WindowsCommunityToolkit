// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Reflection;
using Windows.System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace UnitTests.XamlIslands
{
    class Program
    {
        internal static DispatcherQueue Dispatcher;

        internal static MainForm MainFormInstance;

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainFormInstance = new MainForm();
            Application.Run(MainFormInstance);

            WriteLineColor("Press any key to close this window . . .");
            Console.Read();
        }

        public class MainForm : Form
        {
            public WindowsXamlHost xamlHost = new WindowsXamlHost();

            public MainForm()
            {
                SuspendLayout();
                xamlHost.AutoSizeMode = AutoSizeMode.GrowOnly;
                xamlHost.Location = new System.Drawing.Point(0, 0);
                xamlHost.Name = "xamlHost";
                xamlHost.Size = new System.Drawing.Size(800, 800);
                xamlHost.TabIndex = 0;
                xamlHost.Text = "xamlHost";
                xamlHost.Dock = DockStyle.Fill;
                xamlHost.ChildChanged += XamlHost_ChildChanged;
                xamlHost.InitialTypeName = "Windows.UI.Xaml.Controls.Frame";
                // 
                // Form1
                // 
                AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                AutoScaleMode = AutoScaleMode.Font;
                ClientSize = new System.Drawing.Size(800, 600);
                Controls.Add(xamlHost);
                Name = "MainForm";
                Text = "Xaml Islands";
                ResumeLayout(false);
            }

            private async void XamlHost_ChildChanged(object sender, EventArgs e)
            {
                (xamlHost.Child as Frame).Background = new SolidColorBrush(Colors.CornflowerBlue);

                Dispatcher = DispatcherQueue.GetForCurrentThread();

                await Task.Delay(1000);

                _ = Task.Run(async () =>
                //_dispatcher.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
                {
                    TestResult testResult = default;

                    Stopwatch sw = new Stopwatch();

                    WriteLineColor("--- Starting Tests Execution ---");

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

                    var color = testResult.Failed == 0 ? ConsoleColor.Green : ConsoleColor.Red;

                    WriteLineColor($"--- Finished Tests Execution ({testResult.Passed}/{testResult.Count}) ---", color);
                    WriteLineColor($"--- Duration - {sw.Elapsed} ---");

                    Application.Exit();
                });
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
                        var top = Console.CursorTop;
                        try
                        {
                            WriteLineColor($"{type.FullName}.{method.Name}\t - \t Running...");

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
                            Console.SetCursorPosition(0, top);
                            WriteLineColor($"{type.FullName}.{method.Name}\t - \t PASS      ", ConsoleColor.Green);
                            passed++;
                        }
                        catch (Exception ex)
                        {
                            Console.SetCursorPosition(0, top);
                            WriteLineColor($"{type.FullName}.{method.Name}\t - \t FAIL      :{Environment.NewLine}{ex}", ConsoleColor.Red);
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

        private static void WriteLineColor(string message, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Debug.WriteLine(message);
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
