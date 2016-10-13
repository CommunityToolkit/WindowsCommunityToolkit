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
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;
using Windows.UI.Popups;

namespace Microsoft.Toolkit.Uwp.SampleApp.Controls
{
    public partial class CodeRenderer : Control
    {
        private ProgressRing _progress;
        private WebView _webView;
        private Button _printButton;

        private bool _isInitialized;

        public CodeRenderer()
        {
            DefaultStyleKey = typeof(CodeRenderer);
        }

        protected override void OnApplyTemplate()
        {
            if (_webView != null)
            {
                _webView.LoadCompleted -= OnLoadCompleted;
            }

            if (_printButton != null)
            {
                _printButton.Click -= PrintButton_Click;
            }

            _progress = GetTemplateChild("progress") as ProgressRing;
            _webView = GetTemplateChild("webView") as WebView;
            _printButton = GetTemplateChild("PrintButton") as Button;

            if (_webView != null)
            {
                _webView.LoadCompleted += OnLoadCompleted;
            }

            if (_printButton != null)
            {
                _printButton.Click += PrintButton_Click;
            }

            _isInitialized = true;

            SetHtmlSource(HtmlSource);
            SetCSharpSource(CSharpSource);
            SetXamlSource(XamlSource);
            SetXmlSource(XmlSource);
            SetJsonSource(JsonSource);

            base.OnApplyTemplate();
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;

            Shell.Current.DisplayWaitRing = true;

            await PrintManager.ShowPrintUIAsync();

            Shell.Current.DisplayWaitRing = false;
        }

        private void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("UWP Community Toolkit Sample App", sourceRequested =>
            {
                // Print Task event handler is invoked when the print job is completed.
                printTask.Completed += async (s, args) =>
                {
                    // Notify the user when the print operation fails.
                    if (args.Completion == PrintTaskCompletion.Failed)
                    {
                        var dialog = new MessageDialog("Failed to print.");
                        await dialog.ShowAsync();
                    }
                };

                var printDocument = new PrintDocument();

                var printDocumentSource = printDocument.DocumentSource;
                printDocument.Paginate += CreatePrintPreviewPages;
                printDocument.GetPreviewPage += GetPrintPreviewPage;
                printDocument.AddPages += AddPrintPages;

                sourceRequested.SetSource(printDocumentSource);
            });
        }

        private void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            // Clear the cache of preview pages
            printPreviewPages.Clear();

            // Clear the print canvas of preview pages
            PrintCanvas.Children.Clear();

            // This variable keeps track of the last RichTextBlockOverflow element that was added to a page which will be printed
            RichTextBlockOverflow lastRTBOOnPage;

            // Get the PrintTaskOptions
            PrintTaskOptions printingOptions = ((PrintTaskOptions)e.PrintTaskOptions);

            // Get the page description to deterimine how big the page is
            PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

            // We know there is at least one page to be printed. passing null as the first parameter to
            // AddOnePrintPreviewPage tells the function to add the first page.
            lastRTBOOnPage = AddOnePrintPreviewPage(null, pageDescription);

            // We know there are more pages to be added as long as the last RichTextBoxOverflow added to a print preview
            // page has extra content
            while (lastRTBOOnPage.HasOverflowContent && lastRTBOOnPage.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                lastRTBOOnPage = AddOnePrintPreviewPage(lastRTBOOnPage, pageDescription);
            }

            if (PreviewPagesCreated != null)
            {
                PreviewPagesCreated.Invoke(printPreviewPages, null);
            }

            PrintDocument printDoc = (PrintDocument)sender;

            // Report the number of preview pages created
            printDoc.SetPreviewPageCount(printPreviewPages.Count, PreviewPageCountType.Intermediate);
        }

        private async Task ShowDocument(string docText, string pattern)
        {
            if (_webView != null)
            {
                HideWebView();

                var patternFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/Html/{pattern}"));
                var patternText = await FileIO.ReadTextAsync(patternFile);

                docText = docText.Replace("<", "&lt;").Replace(">", "&gt;");

                string html = patternText.Replace("[CODE]", docText);

                _webView.NavigateToString(html);
            }
        }

        private void OnLoadCompleted(object sender, NavigationEventArgs e)
        {
            ShowWebView();
        }

        private string IndentXml(string xml)
        {
            try
            {
                var xdoc = XDocument.Parse(xml);

                var sb = new StringBuilder();
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "  ",
                    NewLineChars = "\r\n",
                    NewLineHandling = NewLineHandling.Replace
                };

                using (XmlWriter writer = XmlWriter.Create(sb, settings))
                {
                    xdoc.Save(writer);
                }

                return sb.ToString();
            }
            catch
            {
                return xml;
            }
        }

        private string IndentJson(string json)
        {
            try
            {
                var parsedJson = JsonConvert.DeserializeObject(json);
                string indented = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                return indented;
            }
            catch (Exception)
            {
                return json;
            }
        }

        private void ShowWebView()
        {
            _progress.IsActive = false;
            _webView.Visibility = Visibility.Visible;
        }

        private void HideWebView()
        {
            _progress.IsActive = true;
            _webView.Visibility = Visibility.Collapsed;
        }
    }
}
