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
            this.inkToolbarCustomToolButton1 = new Microsoft.Toolkit.Forms.UI.Controls.InkToolbarCustomToolButton();
            this.inkCanvas1 = new Microsoft.Toolkit.Forms.UI.Controls.InkCanvas();
            this.webTab = new System.Windows.Forms.TabPage();
            this.webViewCompatible1 = new Microsoft.Toolkit.Forms.UI.Controls.WebViewCompatible();
            this.mediaTab = new System.Windows.Forms.TabPage();
            this.mediaPlayerElement1 = new Microsoft.Toolkit.Forms.UI.Controls.MediaPlayerElement();
            this.handWritingViewTab = new System.Windows.Forms.TabPage();
            this.testTab = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.inkingTab.SuspendLayout();
            this.inkToolbar1.SuspendLayout();
            this.webTab.SuspendLayout();
            this.mediaTab.SuspendLayout();
            this.handWritingViewTab.SuspendLayout();
            this.handWritingTabTitleLabel = new System.Windows.Forms.Label();
            this.handWritingTabTextBoxLabel = new System.Windows.Forms.Label();
            this.handWritingTabTextBox = new Microsoft.Toolkit.Forms.UI.Controls.TextBox();
            this.handWritingTabRichEditBoxLabel = new System.Windows.Forms.Label();
            this.handWritingTabRichEditBox = new Microsoft.Toolkit.Forms.UI.Controls.RichEditBox();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.inkingTab);
            this.tabControl1.Controls.Add(this.webTab);
            this.tabControl1.Controls.Add(this.mediaTab);
            this.tabControl1.Controls.Add(this.handWritingViewTab);
            this.tabControl1.Controls.Add(this.testTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1059, 707);
            this.tabControl1.TabIndex = 0;
            // 
            // inkingTab
            // 
            this.inkingTab.Controls.Add(this.inkToolbar1);
            this.inkingTab.Controls.Add(this.inkCanvas1);
            this.inkingTab.Location = new System.Drawing.Point(4, 22);
            this.inkingTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.inkingTab.Name = "inkingTab";
            this.inkingTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.inkingTab.Size = new System.Drawing.Size(1051, 681);
            this.inkingTab.TabIndex = 1;
            this.inkingTab.Text = "Ink Canvas / Toolbar";
            this.inkingTab.UseVisualStyleBackColor = true;
            // 
            // inkToolbar1
            // 
            this.inkToolbar1.ActiveTool = this.inkToolbarCustomToolButton1;
            this.inkToolbar1.AutoSize = true;
            this.inkToolbar1.ButtonFlyoutPlacement = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.InkToolbarButtonFlyoutPlacement.Bottom;
            this.inkToolbar1.Controls.Add(this.inkToolbarCustomToolButton1);
            this.inkToolbar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.inkToolbar1.InitialControls = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.InkToolbarInitialControls.All;
            this.inkToolbar1.IsRulerButtonChecked = false;
            this.inkToolbar1.IsStencilButtonChecked = false;
            this.inkToolbar1.Location = new System.Drawing.Point(2, 2);
            this.inkToolbar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.inkToolbar1.MinimumSize = new System.Drawing.Size(13, 39);
            this.inkToolbar1.Name = "inkToolbar1";
            this.inkToolbar1.Orientation = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.Orientation.Horizontal;
            this.inkToolbar1.Size = new System.Drawing.Size(1047, 39);
            this.inkToolbar1.TabIndex = 2;
            this.inkToolbar1.TargetInkCanvas = this.inkCanvas1;
            this.inkToolbar1.Text = "inkToolbar1";
            // 
            // inkToolbarCustomToolButton1
            // 
            this.inkToolbarCustomToolButton1.ConfigurationContent = null;
            this.inkToolbarCustomToolButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.inkToolbarCustomToolButton1.IsExtensionGlyphShown = false;
            this.inkToolbarCustomToolButton1.Location = new System.Drawing.Point(997, 0);
            this.inkToolbarCustomToolButton1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.inkToolbarCustomToolButton1.MinimumSize = new System.Drawing.Size(13, 13);
            this.inkToolbarCustomToolButton1.Name = "inkToolbarCustomToolButton1";
            this.inkToolbarCustomToolButton1.Size = new System.Drawing.Size(50, 39);
            this.inkToolbarCustomToolButton1.TabIndex = 0;
            this.inkToolbarCustomToolButton1.Text = "inkToolbarCustomToolButton1";
            // 
            // inkCanvas1
            // 
            this.inkCanvas1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inkCanvas1.Location = new System.Drawing.Point(2, 2);
            this.inkCanvas1.Name = "inkCanvas1";
            this.inkCanvas1.Size = new System.Drawing.Size(1047, 677);
            this.inkCanvas1.TabIndex = 1;
            this.inkCanvas1.Text = "inkCanvas1";
            // 
            // webTab
            // 
            this.webTab.Controls.Add(this.webViewCompatible1);
            this.webTab.Location = new System.Drawing.Point(4, 22);
            this.webTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.webTab.Name = "webTab";
            this.webTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.webTab.Size = new System.Drawing.Size(1051, 681);
            this.webTab.TabIndex = 0;
            this.webTab.Text = "WebViewCompatible";
            this.webTab.UseVisualStyleBackColor = true;
            // 
            // webViewCompatible1
            // 
            this.webViewCompatible1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewCompatible1.Location = new System.Drawing.Point(2, 2);
            this.webViewCompatible1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.webViewCompatible1.Name = "webViewCompatible1";
            this.webViewCompatible1.Size = new System.Drawing.Size(1047, 677);
            this.webViewCompatible1.Source = new System.Uri("http://www.bing.com", System.UriKind.Absolute);
            this.webViewCompatible1.TabIndex = 0;
            this.webViewCompatible1.Text = "webViewCompatible1";
            // 
            // mediaTab
            // 
            this.mediaTab.Controls.Add(this.mediaPlayerElement1);
            this.mediaTab.Location = new System.Drawing.Point(4, 22);
            this.mediaTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mediaTab.Name = "mediaTab";
            this.mediaTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mediaTab.Size = new System.Drawing.Size(1051, 681);
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
            this.mediaPlayerElement1.Location = new System.Drawing.Point(2, 2);
            this.mediaPlayerElement1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.mediaPlayerElement1.MinimumSize = new System.Drawing.Size(67, 65);
            this.mediaPlayerElement1.Name = "mediaPlayerElement1";
            this.mediaPlayerElement1.Size = new System.Drawing.Size(1047, 677);
            this.mediaPlayerElement1.Source = "https://mediaplatstorage1.blob.core.windows.net/windows-universal-samples-media/e" +
    "lephantsdream-clip-h264_sd-aac_eng-aac_spa-aac_eng_commentary-srt_eng-srt_por-sr" +
    "t_swe.mkv";
            this.mediaPlayerElement1.Stretch = Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT.Stretch.Uniform;
            this.mediaPlayerElement1.TabIndex = 0;
            this.mediaPlayerElement1.Text = "mediaPlayerElement1";
            // 
            // testTab
            // 
            this.testTab.Location = new System.Drawing.Point(4, 22);
            this.testTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.testTab.Name = "testTab";
            this.testTab.Size = new System.Drawing.Size(1051, 681);
            this.testTab.TabIndex = 3;
            this.testTab.Text = "Test";
            this.testTab.UseVisualStyleBackColor = true;
            //
            // handwritingViewTab
            //
            this.handWritingViewTab.Location = new System.Drawing.Point(4, 22);
            this.handWritingViewTab.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.handWritingViewTab.Name = "textBoxHandwritingView";
            this.handWritingViewTab.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.handWritingViewTab.Size = new System.Drawing.Size(1051, 681);
            this.handWritingViewTab.TabIndex = 4;
            this.handWritingViewTab.Text = "TextBox.HandwritingView";
            this.handWritingViewTab.UseVisualStyleBackColor = true;
            this.handWritingViewTab.Controls.Add(this.handWritingTabRichEditBox);
            this.handWritingViewTab.Controls.Add(this.handWritingTabRichEditBoxLabel);
            this.handWritingViewTab.Controls.Add(this.handWritingTabTextBox);
            this.handWritingViewTab.Controls.Add(this.handWritingTabTextBoxLabel);
            this.handWritingViewTab.Controls.Add(this.handWritingTabTitleLabel);
            //
            // handWritingTabTitleLabel
            // 
            this.handWritingTabTitleLabel.AutoSize = true;
            this.handWritingTabTitleLabel.Location = new System.Drawing.Point(5, 4);
            this.handWritingTabTitleLabel.Name = "handWritingTabTitleLabel";
            this.handWritingTabTitleLabel.Size = new System.Drawing.Size(400, 13);
            this.handWritingTabTitleLabel.TabIndex = 0;
            this.handWritingTabTitleLabel.Text = "Tap in the below TextBox or RichEditBox with the pen to use the HandWritingView.";
            // 
            // handWritingTabTextBoxLabel
            // 
            this.handWritingTabTextBoxLabel.AutoSize = true;
            this.handWritingTabTextBoxLabel.Location = new System.Drawing.Point(5, 31);
            this.handWritingTabTextBoxLabel.Name = "handWritingTabTextBoxLabel";
            this.handWritingTabTextBoxLabel.Size = new System.Drawing.Size(49, 13);
            this.handWritingTabTextBoxLabel.TabIndex = 1;
            this.handWritingTabTextBoxLabel.Text = "TextBox:";
            // 
            // handWritingTabTextBox
            // 
            this.handWritingTabTextBox.Location = new System.Drawing.Point(8, 47);
            this.handWritingTabTextBox.Name = "handWritingTabTextBox";
            this.handWritingTabTextBox.Size = new System.Drawing.Size(400, 50);
            this.handWritingTabTextBox.TabIndex = 2;
            this.handWritingTabTextBox.PlaceholderText = "This is an UWP TextBox Control";
            // 
            // handWritingTabRichEditBoxLabel
            // 
            this.handWritingTabRichEditBoxLabel.AutoSize = true;
            this.handWritingTabRichEditBoxLabel.Location = new System.Drawing.Point(5, 189);
            this.handWritingTabRichEditBoxLabel.Name = "handWritingTabRichEditBoxLabel";
            this.handWritingTabRichEditBoxLabel.Size = new System.Drawing.Size(65, 13);
            this.handWritingTabRichEditBoxLabel.TabIndex = 3;
            this.handWritingTabRichEditBoxLabel.Text = "RichEditBox";
            // 
            // handWritingTabRichEditBox
            // 
            this.handWritingTabRichEditBox.Location = new System.Drawing.Point(8, 205);
            this.handWritingTabRichEditBox.Name = "handWritingTabRichEditBox";
            this.handWritingTabRichEditBox.Size = new System.Drawing.Size(400, 50);
            this.handWritingTabRichEditBox.TabIndex = 4;
            this.handWritingTabRichEditBox.PlaceholderText = "This is an UWP RichEditBox Control";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 707);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.TabPage handWritingViewTab;
        private Forms.UI.Controls.WebViewCompatible webViewCompatible1;
        private Forms.UI.Controls.InkCanvas inkCanvas1;
        private System.Windows.Forms.TabPage mediaTab;
        private Forms.UI.Controls.MediaPlayerElement mediaPlayerElement1;
        private System.Windows.Forms.TabPage testTab;
        private Forms.UI.Controls.InkToolbar inkToolbar1;
        private Forms.UI.Controls.InkToolbarCustomToolButton inkToolbarCustomToolButton1;
        private System.Windows.Forms.Label handWritingTabTitleLabel;
        private System.Windows.Forms.Label handWritingTabTextBoxLabel;
        private Forms.UI.Controls.TextBox handWritingTabTextBox;
        private System.Windows.Forms.Label handWritingTabRichEditBoxLabel;
        private Forms.UI.Controls.RichEditBox handWritingTabRichEditBox;
    }
}

