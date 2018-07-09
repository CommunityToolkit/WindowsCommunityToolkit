// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests
{
    [DebuggerStepThrough]
    public abstract class BlockTestStartEndContextSpecification : WebViewContextSpecification
    {
        protected override void Given()
        {
            PrintStartEnd(
                TestContext.TestName,
                nameof(Given),
                () =>
                {
                    TryAction(WaitForWindowsExceptionReporting);
                    base.Given();

                    TryAction(() =>
                    {
                        if (WebView.Process == null) return;

                        var process = Process.GetProcessById((int)WebView.Process.ProcessId);
                        if (process != null)
                        {
                            WriteLine($"[{process.Id}] {process.ProcessName}");
                        }

                    });
                });
        }



        protected override void Cleanup()
        {
            PrintStartEnd(
                TestContext.TestName,
                nameof(Cleanup),
                () =>
                {
                    TryAction(WaitForWindowsExceptionReporting);

                    // The layer above us should have cleaned everything up; if not log information about it here and clean up for the next test
                    TryAction(() =>
                    {
                        if (WebView?.Process != null)
                        {
                            var closeRequested = false;
                            var closeMainWindowRequested = false;
                            var orphan = false;

                            TryAction(PrintProcessDetails);

                            while (WebView.Process.ProcessId != 0)
                            {
                                orphan = true;
                                var process = Process.GetProcessById((int)WebView.Process.ProcessId);
                                if (process != null)
                                {
                                    WriteLine(
                                        $"{process.ProcessName} (PID: {process.Id}) found; waiting for exit...");

                                    if (!process.WaitForExit(TestConstants.Timeouts.Shorter))
                                    {
                                        var msg = $"{process.ProcessName} (PID: {process.Id}) did not exit as expected; ";
                                        if (!closeRequested && !closeMainWindowRequested)
                                        {
                                            closeRequested = true;
                                            msg += " Process.Close requested...";
                                            process.Close();
                                        }
                                        else if (closeRequested && !closeMainWindowRequested)
                                        {
                                            closeMainWindowRequested = true;
                                            msg += " Process.CloseMainWindow requested...";
                                            process.CloseMainWindow();
                                        }
                                        else
                                        {
                                            msg += " Process.Kill requested...";
                                            process.Kill();
                                        }
                                        WriteLine(msg);
                                    }
                                }
                            }

                            if (!orphan)
                            {
                                WriteLine("WebView processes were cleaned up as expected");
                            }
                        }

                        base.Cleanup();
                    });
                });
        }

        private void PrintProcessDetails(string processName)
        {
            var processes = Process.GetProcessesByName(processName);
            var msg = $"Running instances of {processName}.exe: ";
            if (processes.Length == 0)
            {
                msg += " NONE\r\n";
                WriteLine(msg);
            }
            else
            {
                WriteLine(msg);
                var header = "\r\n\r\nName\t\t\tPID\tTitle\r\n------------------------------------------------";
                WriteLine(header);
                foreach (var process in processes)
                {
                    WriteLine($"{process.ProcessName}\t{process.Id}\t{process.MainWindowTitle}");
                }

                WriteLine("");
            }
        }

        private void PrintProcessDetails()
        {
            PrintProcessDetails("WWAHost");
            PrintProcessDetails("Win32WebViewHost");
        }

        private void WaitForWindowsExceptionReporting()
        {
            // We may have had a crash, wait until WER is completed
            var processes = Process.GetProcessesByName("WerFault");
            if (processes.Any())
            {
                var msg = $"Found {processes.Length} instances of WerFault.exe; waiting for those to exit...";
                WriteLine(msg);

                var tasks = new Task[processes.Length];
                for (var i = 0; i < processes.Length; i++)
                {
                    var p = processes[i];
                    var pid = p.Id;
                    var name = p.ProcessName;
                    var cl = p.GetCommandLine();
                    var testName = string.Empty;
                    var regexMatch = Regex.Match(cl, @"-p\s(?<id>\d{1,7})", RegexOptions.IgnoreCase | RegexOptions.Singleline);

                    if (regexMatch.Success)
                    {
                        if (uint.TryParse(regexMatch.Groups["id"].Value, out var testPid))
                        {
                            // NOTE: The PID for Win32WebViewHost.exe is not exposed, so we can't wait for that
                            var testForProcessId = GetTestNameForProcessId(testPid);
                            if (!string.IsNullOrEmpty(testForProcessId))
                            {
                                testName = testForProcessId;
                            }
                        }
                    }

                    msg = $"\t[{pid}] {name} (\"{cl}\") {testName}";
                    WriteLine(msg);

                    // Only wait if WER is gathering information from one of our tests
                    if (string.IsNullOrEmpty(testName))
                    {
                        tasks[i] = Task.CompletedTask;
                    }
                    else
                    {
                        tasks[i] = Task.Run(() => p.WaitForExit());
                    }
                }


                Task.WaitAll(tasks);
            }
        }




    }


}

