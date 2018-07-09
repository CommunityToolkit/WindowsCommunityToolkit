// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using ColorCode;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    public class CodeRenderer : Control
    {
        private RichTextBlockFormatter _formatter;
        private RichTextBlock _codeView;
        private Button _printButton;
        private Button _copyButton;
        private PrintHelper _printHelper;
        private Grid _container;

        private string _displayedText;
        private ILanguage _language;
        private bool _rendered;

        public CodeRenderer()
        {
            DefaultStyleKey = typeof(CodeRenderer);
        }

        /// <summary>
        /// Renders the Code with Syntax Highlighting of the provided Language Alias.
        /// </summary>
        /// <param name="code">Code to Render</param>
        /// <param name="language">Language Alias</param>
        public void SetCode(string code, string language)
        {
            _rendered = false;
            _language = Languages.FindById(language);
            _displayedText = code;

            if (_codeView != null)
            {
                RenderDocument();
            }
        }

        protected override void OnApplyTemplate()
        {
            if (_printButton != null)
            {
                _printButton.Click -= PrintButton_Click;
            }

            if (_copyButton != null)
            {
                _copyButton.Click -= CopyButton_Click;
            }

            _codeView = GetTemplateChild("codeView") as RichTextBlock;
            _printButton = GetTemplateChild("PrintButton") as Button;
            _copyButton = GetTemplateChild("CopyButton") as Button;
            _container = GetTemplateChild("Container") as Grid;

            if (_printButton != null)
            {
                _printButton.Click += PrintButton_Click;
            }

            if (_copyButton != null)
            {
                _copyButton.Click += CopyButton_Click;
            }

            if (!_rendered)
            {
                RenderDocument();
            }

            base.OnApplyTemplate();
        }

        private void RenderDocument()
        {
            _codeView.Blocks.Clear();
            _formatter = new RichTextBlockFormatter();
            _formatter.FormatRichTextBlock(_displayedText, _language, _codeView);
            _rendered = true;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            TrackingManager.TrackEvent("Copy", _displayedText);

            var content = new DataPackage();
            content.SetText(_displayedText);
            Clipboard.SetContent(content);
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            Shell.Current.DisplayWaitRing = true;

            var printblock = new RichTextBlock
            {
                FontFamily = _codeView.FontFamily
            };
            _formatter.FormatRichTextBlock(_displayedText, _language, printblock);

            _printHelper = new PrintHelper(_container);
            _printHelper.AddFrameworkElementToPrint(printblock);

            _printHelper.OnPrintFailed += PrintHelper_OnPrintFailed;
            _printHelper.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;
            _printHelper.OnPrintCanceled += PrintHelper_OnPrintCanceled;

            await _printHelper.ShowPrintUIAsync("Windows Community Toolkit Sample App");
        }

        private void ReleasePrintHelper()
        {
            _printHelper.OnPrintFailed -= PrintHelper_OnPrintFailed;
            _printHelper.OnPrintSucceeded -= PrintHelper_OnPrintSucceeded;
            _printHelper.OnPrintCanceled -= PrintHelper_OnPrintCanceled;

            _printHelper.Dispose();

            Shell.Current.DisplayWaitRing = false;
        }

        private async void PrintHelper_OnPrintSucceeded()
        {
            ReleasePrintHelper();
            var dialog = new MessageDialog("Printing done.");
            await dialog.ShowAsync();
        }

        private async void PrintHelper_OnPrintFailed()
        {
            ReleasePrintHelper();
            var dialog = new MessageDialog("Printing failed.");
            await dialog.ShowAsync();
        }

        private void PrintHelper_OnPrintCanceled()
        {
            ReleasePrintHelper();
        }
    }
}