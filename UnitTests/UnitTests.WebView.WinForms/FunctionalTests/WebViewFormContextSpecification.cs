// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Diagnostics;
using System.Windows.Forms;
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


    }
}