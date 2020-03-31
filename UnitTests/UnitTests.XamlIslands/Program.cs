// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Toolkit.Win32.UI.XamlHost;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using System.Reflection;

namespace UnitTests.XamlIslands
{
    class Program
    {
        private static IXamlMetadataContainer _metadataContainer;
        internal static CoreDispatcher _dispatcher;
        private static Task _task;

        [STAThread]
        public static void Main()
        {
            _metadataContainer = XamlApplicationExtensions.GetOrCreateXamlMetadataContainer();

            using (var xamlSource = new DesktopWindowXamlSource())
            {
                var frame = UWPTypeFactory.CreateXamlContentByType("Windows.UI.Xaml.Controls.Frame");
                xamlSource.Content = frame;

                _dispatcher = xamlSource.Content.XamlRoot.Content.Dispatcher;

                _task = Task.Run(async () =>
                {
                    await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        foreach (var testClass in typeof(XamlIslandsTest_ThemeListener_Threading).Assembly.GetTypes())
                        {
                            Attribute[] attributes = Attribute.GetCustomAttributes(testClass);

                            foreach (Attribute attribute in attributes)
                            {
                                if (attribute is STATestClassAttribute || attribute is TestClassAttribute)
                                {
                                    await RunTestsAsync(testClass);
                                    break;
                                }
                            }
                        }
                        _dispatcher.StopProcessEvents();
                        Window.Current.CoreWindow.Close();
                    });
                });

                _dispatcher.ProcessEvents(CoreProcessEventsOption.ProcessUntilQuit);
            }
        }

        private static async Task RunTestsAsync(Type type)
        {
            var initMethod = GetFirstMethod(type, typeof(TestInitializeAttribute));
            var cleanupMethod = GetFirstMethod(type, typeof(TestCleanupAttribute));

            foreach (var method in type.GetMethods())
            {
                Attribute[] attributes = Attribute.GetCustomAttributes(method);

                foreach (Attribute attribute in attributes)
                {
                    if (attribute is STATestMethodAttribute || attribute is TestMethodAttribute)
                    {
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
                        }
                        catch (Exception ex)
                        {
                            TestFail(type, method, ex);
                        }
                        break;
                    }
                }
            }
        }

        private static void TestPass(Type type, MethodInfo method)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{type.FullName}.{method.Name}\t - \tPASS");
            Console.ResetColor();
        }

        private static void TestFail(Type type, MethodInfo method, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{type.FullName}.{method.Name}\t - \tFAIL:{Environment.NewLine}{ex}");
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
