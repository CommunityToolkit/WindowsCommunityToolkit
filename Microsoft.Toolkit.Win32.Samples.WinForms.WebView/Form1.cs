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
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.WinForms;

namespace Microsoft.Toolkit.Win32.Samples.WinForms.WebView
{
    public partial class Form1 : Form
    {
        private bool _isFullScreen;

        public Form1()
        {
            InitializeComponent();
        }

        private void alertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure what we need is set
            webView1.IsIndexedDBEnabled = false;
            webView1.IsJavaScriptEnabled = true;
            webView1.IsScriptNotifyAllowed = true;

            // URI needs to be in the app content URI's section of the package manifest. Since Winforms, not sure
            // We can use NavigateToString to get this to work
            var c = @"
<html>
  <head>
    <title>Alert Intercept</title>
  </head>
  <body>
    <button type=""button"" OnClick=""window.alert('Hello World!');"">Click</button>
  </body>
</html>
";
            void WebView1_NavigationCompleted(object s, WebViewNavigationCompletedEventArgs a)
            {
                webView1.InvokeScriptAsync(
                    "eval",
                    "window.alert = function(msg) { window.external.notify(msg); };");
            }

            webView1.NavigationCompleted += WebView1_NavigationCompleted;
            webView1.NavigateToString(c);
        }



        private void geolocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure what we need is set
            webView1.IsIndexedDBEnabled = false;
            webView1.IsJavaScriptEnabled = true;
            webView1.IsScriptNotifyAllowed = true;
            webView1.Source = new Uri("https://codepen.io/rjmurillo/pen/MVaKbJ");
        }

        private void goBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView1.GoBack();
        }

        private void goForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView1.GoForward();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs args)
        {
            // From https://stackoverflow.com/questions/5427020/prompt-dialog-in-windows-forms
            string ShowDialog(string text, string caption)
            {
                var prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                var textLabel = new Label() {Left = 50, Top = 20, Text = text};
                var textBox = new TextBox() {Left = 50, Top = 50, Width = 400};
                var confirmation = new Button()
                {
                    Text = "Ok",
                    Left = 350,
                    Width = 100,
                    Top = 70,
                    DialogResult = DialogResult.OK
                };
                confirmation.Click += (s, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog(this) == DialogResult.OK ? textBox.Text : "";
            }

            var inputUri = ShowDialog("URL", "URL");
            var uri = (Uri) new WebBrowserUriTypeConverter().ConvertFromString(inputUri);
            webView1.IsIndexedDBEnabled = true;
            webView1.IsScriptNotifyAllowed = true;
            webView1.IsJavaScriptEnabled = true;

            webView1.Navigate(uri);
        }

        private void pointerLockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView1.IsJavaScriptEnabled = true;
            webView1.Navigate(new Uri("https://mdn.github.io/dom-examples/pointer-lock/", UriKind.Absolute));
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView1.Refresh();
        }

        private void screenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView1.IsIndexedDBEnabled = false;
            webView1.IsJavaScriptEnabled = true;
            webView1.IsScriptNotifyAllowed = false;
            webView1.Navigate(new Uri("http://blogs.sitepointstatic.com/examples/tech/full-screen/index2.html"));
        }

        private void scriptNotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView1.IsScriptNotifyAllowed = true;
            webView1.IsJavaScriptEnabled = true;
            webView1.IsIndexedDBEnabled = false;
            // URI needs to be in the app content URI's section of the package manifest. Since Winforms, not sure
            //webView1.Navigate(new Uri("https://codepen.io/rjmurillo/pen/QmNjQV", UriKind.Absolute));

            // We can use NavigateToString to get this to work
            var c = @"
<html>
  <head>
    <title>OnScriptNotify</title>
  </head>
  <body>
    <button type=""button"" OnClick=""window.external.notify('Hello World!');"">Click</button>
  </body>
</html>
";
            webView1.NavigateToString(c);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView1.Stop();
        }

        private void webNotificationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView1.IsJavaScriptEnabled = true;
            webView1.Navigate(new Uri("https://davidwalsh.name/demo/notifications-api.php", UriKind.Absolute));
        }

        private void webView1_ContainsFullScreenElementChanged(object sender, object e)
        {
            void EnterFullScreen()
            {
                menuStrip1.Visible = false;
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }

            void LeaveFullScreen()
            {
                menuStrip1.Visible = true;
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

        private void webView1_NavigationCompleted(object sender, WebViewNavigationCompletedEventArgs e)
        {
            Text = webView1.DocumentTitle;
            if (!e.IsSuccess)
            {
                MessageBox.Show($"Could not navigate to {e.Uri}", $"Error: {e.WebErrorStatus}",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void webView1_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
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