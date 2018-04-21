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
        private ElementTheme _theme;

        public CodeRenderer()
        {
            DefaultStyleKey = typeof(CodeRenderer);
            _theme = Shell.Current.GetActualTheme();
            Shell.Current.ThemeChanged += Current_ThemeChanged;
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
            _codeView?.Blocks?.Clear();
            _formatter = new RichTextBlockFormatter(_theme);
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
            SampleController.Current.DisplayWaitRing = true;

            var printblock = new RichTextBlock
            {
                FontFamily = _codeView.FontFamily,
                RequestedTheme = ElementTheme.Light
            };
            var printFormatter = new RichTextBlockFormatter(ElementTheme.Light);
            printFormatter.FormatRichTextBlock(_displayedText, _language, printblock);

            _printHelper = new PrintHelper(_container);
            _printHelper.AddFrameworkElementToPrint(printblock);

            _printHelper.OnPrintFailed += PrintHelper_OnPrintFailed;
            _printHelper.OnPrintSucceeded += PrintHelper_OnPrintSucceeded;
            _printHelper.OnPrintCanceled += PrintHelper_OnPrintCanceled;

            await _printHelper.ShowPrintUIAsync("UWP Community Toolkit Sample App");
        }

        private void ReleasePrintHelper()
        {
            _printHelper.OnPrintFailed -= PrintHelper_OnPrintFailed;
            _printHelper.OnPrintSucceeded -= PrintHelper_OnPrintSucceeded;
            _printHelper.OnPrintCanceled -= PrintHelper_OnPrintCanceled;

            _printHelper.Dispose();

            SampleController.Current.DisplayWaitRing = false;
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

        private void Current_ThemeChanged(object sender, Models.ThemeChangedArgs e)
        {
            _theme = Shell.Current.GetActualTheme();
            try
            {
                _rendered = false;
                RenderDocument();
            }
            catch { }
        }
    }
}