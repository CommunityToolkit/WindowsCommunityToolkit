// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests
{
    [DebuggerStepThrough]
    public abstract class WebViewFormContextSpecification : BlockTestStartEndContextSpecification
    {
        protected WebViewFormContextSpecification()
        {
            Form = new TestHostForm()
            {
                Width = 1000,
                Height = 800
            };

            Form.MouseEnter += (o, e) => { WriteLine($"Form.MouseEnter"); };
            Form.MouseWheel += (o, e) => { WriteLine($"Form.MouseWheel: {e.Location}"); };
            Form.GotFocus += (o, e) => { WriteLine("Form.GotFocus"); };
            Form.LostFocus += (o, e) => { WriteLine("Form.LostFocus"); };
            Form.KeyPress += (o, e) => { WriteLine($"Form.KeyPress: {e.KeyChar}"); };
            Form.Closing += (o, e) => { WriteLine("Form.Closing"); };
            Form.Closed += (o, e) => { WriteLine("Form.Closed"); };
        }

        protected TestHostForm Form { get; private set; }

        protected override void Cleanup()
        {
            PrintStartEnd(
                TestContext.TestName,
                nameof(Cleanup),
                () =>
                {
                    try
                    {
                        if (!Form.IsDisposed)
                        {
                            // The Form is supposed to be closed when the test is completed (to signal it is done)
                            // If it has not been closed and disposed, go ahead and do that so we can unhook

                            WriteLine("Closing the form instance...");

                            // Restore foreground to the window before ending the test
                            Form.BringToFront();

                            Form.Close();
                            Form.Dispose();
                        }
                    }
                    finally
                    {
                        base.Cleanup();
                    }
                });
        }

        protected override void Given()
        {
            Form.Text = TestContext.TestName;

            base.Given();

            WebView.ShouldNotBeNull();

            WebView.NavigationStarting += (o, e) => { Form.Text = $"{TestContext.TestName}: {e.Uri}" ?? string.Empty; };
            WebView.NavigationCompleted += (o, e) =>
            {
                var focused = WebView.Focus();
                WriteLine($"WebView.Focused: {focused}");
                Application.DoEvents();
            };
        }

        protected virtual void NavigateAndWaitForFormClose(Uri uri)
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WriteLine($"Navigating WebView with URI: {uri}");
                WebView.Navigate(uri);
            });
        }

        protected virtual void NavigateAndWaitForFormClose(
            Uri requestUri,
            HttpMethod httpMethod,
            string content = null,
            IEnumerable<KeyValuePair<string, string>> headers = null)
        {
            PerformActionAndWaitForFormClose(() =>
            {
                string Convert(IEnumerable<KeyValuePair<string, string>> kvp)
                {
                    if (kvp == null)
                    {
                        kvp = Enumerable.Empty<KeyValuePair<string, string>>();
                    }

                    var sb = new StringBuilder();
                    foreach (var k in kvp)
                    {
                        sb.AppendLine($"\r\n    {k.Key}={k.Value}");
                    }

                    return sb.ToString();
                }

                WriteLine(
@"Navigating WebView with
  URI:     {0}
  METHOD:  {1}
  CONTENT: {2}
  HEADERS: {3}",
                    requestUri, httpMethod, content??string.Empty, Convert(headers));
                WebView.Navigate(requestUri, httpMethod, content, headers);
            });
        }

        protected virtual void PerformActionAndWaitForFormClose(Action action)
        {
            void OnFormLoad(object sender, EventArgs e)
            {
                Application.DoEvents();
                action();
            }

            WebView.ShouldNotBeNull();
            Form.Load += OnFormLoad;
            Application.Run(Form);
        }

        protected virtual void NavigateToStringAndWaitForFormClose(string content)
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WriteLine("Navigating WebView with content:");
                WriteLine(content);
                WebView.NavigateToString(content);
            });
        }

        protected virtual void NavigateToLocalAndWaitForFormClose(string relativePath)
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WriteLine("Navigating WebView:");                
#pragma warning disable 618
                WebView.NavigateToLocal(relativePath);
#pragma warning restore 618
            });
        }

        protected virtual void NavigateToLocalAndWaitForFormClose(Uri relativePath, IUriToStreamResolver streamResolver)
        {
            PerformActionAndWaitForFormClose(() =>
            {
                WriteLine("Navigating WebView");
                WebView.NavigateToLocalStreamUri(relativePath, streamResolver);
            });
        }
    }
}