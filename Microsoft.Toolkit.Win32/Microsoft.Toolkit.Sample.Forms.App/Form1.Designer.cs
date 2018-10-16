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
            this.inkingTab = new System.Windows.Forms.TabPage();
            this.inkToolbar1 = new Microsoft.Toolkit.Forms.UI.Controls.InkToolbar();
            this.inkCanvas1 = new Microsoft.Toolkit.Forms.UI.Controls.InkCanvas();
            this.webTab = new System.Windows.Forms.TabPage();
            this.webViewCompatible1 = new Microsoft.Toolkit.Forms.UI.Controls.WebViewCompatible();
            this.mediaTab = new System.Windows.Forms.TabPage();
            this.mediaPlayerElement1 = new Microsoft.Toolkit.Forms.UI.Controls.MediaPlayerElement();
            this.testTab = new System.Windows.Forms.TabPage();
            this.inkToolbarCustomToolButton1 = new Microsoft.Toolkit.Forms.UI.Controls.InkToolbarCustomToolButton();
            this.tabControl1.SuspendLayout();
            this.inkingTab.SuspendLayout();
            this.inkToolbar1.SuspendLayout();
            this.webTab.SuspendLayout();
            this.mediaTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.inkingTab);
            this.tabControl1.Controls.Add(this.webTab);
            this.tabControl1.Controls.Add(this.mediaTab);
            this.tabControl1.Controls.Add(this.testTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1588, 1088);
            this.tabControl1.TabIndex = 0;
            // 
            // inkingTab
            // 
            this.inkingTab.Controls.Add(this.inkToolbar1);
            this.inkingTab.Controls.Add(this.inkCanvas1);
            this.inkingTab.Location = new System.Drawing.Point(4, 29);
            this.inkingTab.Name = "inkingTab";
            this.inkingTab.Padding = new System.Windows.Forms.Padding(3);
            this.inkingTab.Size = new System.Drawing.Size(1580, 1055);
            this.inkingTab.TabIndex = 1;
            this.inkingTab.Text = "Ink Canvas / Toolbar";
            this.inkingTab.UseVisualStyleBackColor = true;
            // 
            // inkToolbar1
            // 
            this.inkToolbar1.Controls.Add(this.inkToolbarCustomToolButton1);
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
            this.inkCanvas1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inkCanvas1.Location = new System.Drawing.Point(3, 3);
            this.inkCanvas1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.inkCanvas1.Name = "inkCanvas1";
            this.inkCanvas1.Size = new System.Drawing.Size(1574, 1049);
            this.inkCanvas1.TabIndex = 1;
            this.inkCanvas1.Text = "inkCanvas1";
            // 
            // webTab
            // 
            this.webTab.Controls.Add(this.webViewCompatible1);
            this.webTab.Location = new System.Drawing.Point(4, 29);
            this.webTab.Name = "webTab";
            this.webTab.Padding = new System.Windows.Forms.Padding(3);
            this.webTab.Size = new System.Drawing.Size(1580, 1055);
            this.webTab.TabIndex = 0;
            this.webTab.Text = "WebViewCompatible";
            this.webTab.UseVisualStyleBackColor = true;
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
            // 
            // mediaTab
            // 
            this.mediaTab.Controls.Add(this.mediaPlayerElement1);
            this.mediaTab.Location = new System.Drawing.Point(4, 29);
            this.mediaTab.Name = "mediaTab";
            this.mediaTab.Padding = new System.Windows.Forms.Padding(3);
            this.mediaTab.Size = new System.Drawing.Size(1580, 1055);
            this.mediaTab.TabIndex = 2;
            this.mediaTab.Text = "MediaPlayerElement";
            this.mediaTab.UseVisualStyleBackColor = true;
            // 
            // mediaPlayerElement1
            // 
            this.mediaPlayerElement1.AreTransportControlsEnabled = true;
            this.mediaPlayerElement1.AutoPlay = true;
            this.mediaPlayerElement1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mediaPlayerElement1.IsFullWindow = false;
            this.mediaPlayerElement1.Location = new System.Drawing.Point(3, 3);
            this.mediaPlayerElement1.MinimumSize = new System.Drawing.Size(100, 100);
            this.mediaPlayerElement1.Name = "mediaPlayerElement1";
            this.mediaPlayerElement1.Size = new System.Drawing.Size(1574, 1049);
            this.mediaPlayerElement1.Source = "https://mediaplatstorage1.blob.core.windows.net/windows-universal-samples-media/e" +
    "lephantsdream-clip-h264_sd-aac_eng-aac_spa-aac_eng_commentary-srt_eng-srt_por-sr" +
    "t_swe.mkv";
            this.mediaPlayerElement1.Stretch = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.Stretch.Uniform;
            this.mediaPlayerElement1.TabIndex = 0;
            this.mediaPlayerElement1.Text = "mediaPlayerElement1";
            // 
            // testTab
            // 
            this.testTab.Location = new System.Drawing.Point(4, 29);
            this.testTab.Name = "testTab";
            this.testTab.Size = new System.Drawing.Size(1580, 1055);
            this.testTab.TabIndex = 3;
            this.testTab.Text = "Test";
            this.testTab.UseVisualStyleBackColor = true;
            // 
            // inkToolbarCustomToolButton1
            // 
            this.inkToolbarCustomToolButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.inkToolbarCustomToolButton1.Location = new System.Drawing.Point(1499, 0);
            this.inkToolbarCustomToolButton1.MinimumSize = new System.Drawing.Size(20, 20);
            this.inkToolbarCustomToolButton1.Name = "inkToolbarCustomToolButton1";
            this.inkToolbarCustomToolButton1.Size = new System.Drawing.Size(75, 60);
            this.inkToolbarCustomToolButton1.TabIndex = 0;
            this.inkToolbarCustomToolButton1.Text = "inkToolbarCustomToolButton1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1588, 1088);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.inkingTab.ResumeLayout(false);
            this.inkToolbar1.ResumeLayout(false);
            this.webTab.ResumeLayout(false);
            this.mediaTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage webTab;
        private System.Windows.Forms.TabPage inkingTab;
        private Forms.UI.Controls.WebViewCompatible webViewCompatible1;
        private Forms.UI.Controls.InkCanvas inkCanvas1;
        private System.Windows.Forms.TabPage mediaTab;
        private Forms.UI.Controls.MediaPlayerElement mediaPlayerElement1;
        private System.Windows.Forms.TabPage testTab;
        private Forms.UI.Controls.InkToolbar inkToolbar1;
        private Forms.UI.Controls.InkToolbarCustomToolButton inkToolbarCustomToolButton1;
    }
}

