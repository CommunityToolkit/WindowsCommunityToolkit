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

using System.Windows.Forms;

namespace Microsoft.Toolkit.Win32.Samples.WinForms.WebView
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goForwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.permissionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geolocationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.unlimitedIndexDBQuotaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointerLockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webNotificationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.screenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scriptNotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webView1 = new Microsoft.Toolkit.Win32.UI.Controls.WinForms.WebView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.demosToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(761, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goBackToolStripMenuItem,
            this.goForwardToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // goBackToolStripMenuItem
            // 
            this.goBackToolStripMenuItem.Name = "goBackToolStripMenuItem";
            this.goBackToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Left)));
            this.goBackToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.goBackToolStripMenuItem.Text = "Go Back";
            this.goBackToolStripMenuItem.Click += new System.EventHandler(this.goBackToolStripMenuItem_Click);
            // 
            // goForwardToolStripMenuItem
            // 
            this.goForwardToolStripMenuItem.Name = "goForwardToolStripMenuItem";
            this.goForwardToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Right)));
            this.goForwardToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.goForwardToolStripMenuItem.Text = "Go Forward";
            this.goForwardToolStripMenuItem.Click += new System.EventHandler(this.goForwardToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // demosToolStripMenuItem
            // 
            this.demosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alertToolStripMenuItem,
            this.permissionsToolStripMenuItem,
            this.scriptNotifyToolStripMenuItem});
            this.demosToolStripMenuItem.Name = "demosToolStripMenuItem";
            this.demosToolStripMenuItem.Size = new System.Drawing.Size(56, 22);
            this.demosToolStripMenuItem.Text = "&Demos";
            // 
            // alertToolStripMenuItem
            // 
            this.alertToolStripMenuItem.Name = "alertToolStripMenuItem";
            this.alertToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.alertToolStripMenuItem.Text = "&Alert";
            this.alertToolStripMenuItem.Click += new System.EventHandler(this.alertToolStripMenuItem_Click);
            // 
            // permissionsToolStripMenuItem
            // 
            this.permissionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geolocationToolStripMenuItem1,
            this.unlimitedIndexDBQuotaToolStripMenuItem,
            this.mediaToolStripMenuItem,
            this.pointerLockToolStripMenuItem,
            this.webNotificationsToolStripMenuItem,
            this.screenToolStripMenuItem});
            this.permissionsToolStripMenuItem.Name = "permissionsToolStripMenuItem";
            this.permissionsToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.permissionsToolStripMenuItem.Text = "&Permissions";
            // 
            // geolocationToolStripMenuItem1
            // 
            this.geolocationToolStripMenuItem1.Name = "geolocationToolStripMenuItem1";
            this.geolocationToolStripMenuItem1.Size = new System.Drawing.Size(208, 22);
            this.geolocationToolStripMenuItem1.Text = "&Geolocation";
            this.geolocationToolStripMenuItem1.Click += new System.EventHandler(this.geolocationToolStripMenuItem_Click);
            // 
            // unlimitedIndexDBQuotaToolStripMenuItem
            // 
            this.unlimitedIndexDBQuotaToolStripMenuItem.Enabled = false;
            this.unlimitedIndexDBQuotaToolStripMenuItem.Name = "unlimitedIndexDBQuotaToolStripMenuItem";
            this.unlimitedIndexDBQuotaToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.unlimitedIndexDBQuotaToolStripMenuItem.Text = "&Unlimited IndexDB Quota";
            // 
            // mediaToolStripMenuItem
            // 
            this.mediaToolStripMenuItem.Enabled = false;
            this.mediaToolStripMenuItem.Name = "mediaToolStripMenuItem";
            this.mediaToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.mediaToolStripMenuItem.Text = "&Media";
            // 
            // pointerLockToolStripMenuItem
            // 
            this.pointerLockToolStripMenuItem.Name = "pointerLockToolStripMenuItem";
            this.pointerLockToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.pointerLockToolStripMenuItem.Text = "P&ointer Lock";
            this.pointerLockToolStripMenuItem.Click += new System.EventHandler(this.pointerLockToolStripMenuItem_Click);
            // 
            // webNotificationsToolStripMenuItem
            // 
            this.webNotificationsToolStripMenuItem.Name = "webNotificationsToolStripMenuItem";
            this.webNotificationsToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.webNotificationsToolStripMenuItem.Text = "&Web Notifications";
            this.webNotificationsToolStripMenuItem.Click += new System.EventHandler(this.webNotificationsToolStripMenuItem_Click);
            // 
            // screenToolStripMenuItem
            // 
            this.screenToolStripMenuItem.Name = "screenToolStripMenuItem";
            this.screenToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.screenToolStripMenuItem.Text = "&Screen";
            this.screenToolStripMenuItem.Click += new System.EventHandler(this.screenToolStripMenuItem_Click);
            // 
            // scriptNotifyToolStripMenuItem
            // 
            this.scriptNotifyToolStripMenuItem.Name = "scriptNotifyToolStripMenuItem";
            this.scriptNotifyToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.scriptNotifyToolStripMenuItem.Text = "&Script Notify";
            this.scriptNotifyToolStripMenuItem.Click += new System.EventHandler(this.scriptNotifyToolStripMenuItem_Click);
            // 
            // webView1
            // 
            this.webView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webView1.IsJavaScriptEnabled = true;
            this.webView1.IsPrivateNetworkClientServerCapabilityEnabled = true;
            this.webView1.IsScriptNotifyAllowed = true;
            this.webView1.Location = new System.Drawing.Point(0, 24);
            this.webView1.Margin = new System.Windows.Forms.Padding(2);
            this.webView1.Name = "webView1";
            this.webView1.Size = new System.Drawing.Size(761, 552);
            this.webView1.Source = new System.Uri("http://bing.com", System.UriKind.Absolute);
            this.webView1.TabIndex = 1;
            this.webView1.ContainsFullScreenElementChanged += new System.EventHandler<object>(this.webView1_ContainsFullScreenElementChanged);
            this.webView1.NavigationCompleted += new System.EventHandler<Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewNavigationCompletedEventArgs>(this.webView1_NavigationCompleted);
            this.webView1.NavigationStarting += new System.EventHandler<Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlNavigationStartingEventArgs>(this.webView1_NavigationStarting);
            this.webView1.PermissionRequested += new System.EventHandler<Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlPermissionRequestedEventArgs>(this.webView1_PermissionRequested);
            this.webView1.ScriptNotify += new System.EventHandler<Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlScriptNotifyEventArgs>(this.webView1_ScriptNotify);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 576);
            this.Controls.Add(this.webView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "WinForms WebView";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private UI.Controls.WinForms.WebView webView1;
        private ToolStripMenuItem stopToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripMenuItem demosToolStripMenuItem;
        private ToolStripMenuItem alertToolStripMenuItem;
        private ToolStripMenuItem permissionsToolStripMenuItem;
        private ToolStripMenuItem geolocationToolStripMenuItem1;
        private ToolStripMenuItem unlimitedIndexDBQuotaToolStripMenuItem;
        private ToolStripMenuItem mediaToolStripMenuItem;
        private ToolStripMenuItem pointerLockToolStripMenuItem;
        private ToolStripMenuItem webNotificationsToolStripMenuItem;
        private ToolStripMenuItem screenToolStripMenuItem;
        private ToolStripMenuItem goBackToolStripMenuItem;
        private ToolStripMenuItem goForwardToolStripMenuItem;
        private ToolStripMenuItem scriptNotifyToolStripMenuItem;
    }
}