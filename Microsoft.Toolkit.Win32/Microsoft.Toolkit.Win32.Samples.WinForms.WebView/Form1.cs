// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

namespace Microsoft.Toolkit.Win32.Samples.WinForms.WebView
{
    public partial class Form1 : Form
    {
        private bool _isFullScreen;

        private bool _processExitedAttached;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webView1?.GoBack();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            webView1?.GoForward();
        }

        private void Go_Click(object sender, EventArgs e)
        {
            var result = (Uri)new WebBrowserUriTypeConverter().ConvertFromString(url.Text);
            webView1.Source = result;
        }

        private void OnFormLoaded(object sender, EventArgs e)
        {
            TryAttachProcessExitedEventHandler();
        }

        private void TryAttachProcessExitedEventHandler()
        {
            if (!_processExitedAttached && webView1?.Process != null)
            {
                webView1.Process.ProcessExited += (o, a) =>
                {
                    //WebView has encountered and error and was terminated
                    Close();
                };

                _processExitedAttached = true;
            }
        }

        private void url_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && webView1 != null)
            {
                var result = (Uri)new WebBrowserUriTypeConverter().ConvertFromString(url.Text);
                webView1.Source = result;
            }
        }

        private void webView1_ContainsFullScreenElementChanged(object sender, object e)
        {
            void EnterFullScreen()
            {
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            void LeaveFullScreen()
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
            }

            // Toggle
            _isFullScreen = !_isFullScreen;

            if (_isFullScreen)
            {
                EnterFullScreen();
            }
            else
            {
                LeaveFullScreen();
            }
        }

        private void webView1_NavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
        {
            TryAttachProcessExitedEventHandler();
            url.Text = e.Uri?.ToString() ?? string.Empty;
            Text = webView1.DocumentTitle;
            if (!e.IsSuccess)
            {
                MessageBox.Show($"Could not navigate to {e.Uri}", $"Error: {e.WebErrorStatus}",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void webView1_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            Text = "Navigating " + e.Uri?.ToString() ?? string.Empty;
            url.Text = e.Uri?.ToString() ?? string.Empty;
        }

        private void webView1_PermissionRequested(object sender, WebViewControlPermissionRequestedEventArgs e)
        {
            if (e.PermissionRequest.State == WebViewControlPermissionState.Allow) return;

            var msg = $"Allow {e.PermissionRequest.Uri.Host} to access {e.PermissionRequest.PermissionType}?";

            var response = MessageBox.Show(msg, "Permission Request", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            if (response == DialogResult.Yes)
            {
                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                    webView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Allow();
                }
                else
                {
                    e.PermissionRequest.Allow();
                }
            }
            else
            {
                if (e.PermissionRequest.State == WebViewControlPermissionState.Defer)
                {
                    webView1.GetDeferredPermissionRequestById(e.PermissionRequest.Id)?.Deny();
                }
                else
                {
                    e.PermissionRequest.Deny();
                }
            }
        }

        private void webView1_ScriptNotify(object sender, WebViewControlScriptNotifyEventArgs e)
        {
            MessageBox.Show(e.Value, e.Uri?.ToString() ?? string.Empty);
        }
    }
}