// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.Samples.WinForms.App
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.webViewCompatible1 = new Microsoft.Toolkit.Win32.UI.Controls.WinForms.WebViewCompatible();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.inkToolbar1 = new Microsoft.Toolkit.Win32.UI.Controls.WinForms.InkToolbar();
            this.inkCanvas1 = new Microsoft.Toolkit.Win32.UI.Controls.WinForms.InkCanvas();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1588, 1088);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.webViewCompatible1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1580, 1055);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "WebViewCompatible";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // webViewCompatible1
            // 
            this.webViewCompatible1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewCompatible1.Location = new System.Drawing.Point(3, 3);
            this.webViewCompatible1.Name = "webViewCompatible1";
            this.webViewCompatible1.Size = new System.Drawing.Size(1574, 1049);
            this.webViewCompatible1.Source = new System.Uri("http://www.bing.com", System.UriKind.Absolute);
            this.webViewCompatible1.TabIndex = 0;
            this.webViewCompatible1.Text = "webViewCompatible1";
            this.webViewCompatible1.ContentLoading += new System.EventHandler<Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.WebViewControlContentLoadingEventArgs>(this.webViewCompatible1_ContentLoading);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.inkToolbar1);
            this.tabPage2.Controls.Add(this.inkCanvas1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1580, 1055);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Ink Canvas / Toolbar";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // inkToolbar1
            // 
            this.inkToolbar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.inkToolbar1.Location = new System.Drawing.Point(3, 3);
            this.inkToolbar1.MinimumSize = new System.Drawing.Size(20, 60);
            this.inkToolbar1.Name = "inkToolbar1";
            this.inkToolbar1.Size = new System.Drawing.Size(1574, 60);
            this.inkToolbar1.TabIndex = 2;
            this.inkToolbar1.TargetInkCanvas = this.inkCanvas1;
            this.inkToolbar1.Text = "inkToolbar1";
            // 
            // inkCanvas1
            // 
            this.inkCanvas1.Location = new System.Drawing.Point(3, 249);
            this.inkCanvas1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inkCanvas1.Name = "inkCanvas1";
            this.inkCanvas1.Size = new System.Drawing.Size(1570, 796);
            this.inkCanvas1.TabIndex = 1;
            this.inkCanvas1.Text = "inkCanvas1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1588, 1088);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private UI.Controls.WinForms.WebViewCompatible webViewCompatible1;
        private UI.Controls.WinForms.InkCanvas inkCanvas1;
        private UI.Controls.WinForms.InkToolbar inkToolbar1;
    }
}

